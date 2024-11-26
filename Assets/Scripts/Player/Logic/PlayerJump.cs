using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerJump : MonoBehaviourPun
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerGroundHandler playerGroundHandler;
    [SerializeField] private PlayerCrouch playerCrouch;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private bool jumpWhileCrouching;
    private float jumpCooldownTimer;

    [Header("Better Jump")]
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;

    private Rigidbody _rigidBody;

    private bool JumpPressed => NewMovementInput.Instance.GetJumpDown();
    private bool shouldJump;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!CanProcessActions()) return;

        JumpUpdateLogic();
    }

    private void FixedUpdate()
    {
        if (!CanProcessActions()) return;

        JumpFixedUpdateLogic();
    }

    private void JumpUpdateLogic()
    {
        if (jumpCooldownTimer > 0) jumpCooldownTimer -= Time.deltaTime;
        else playerGroundHandler.SetExitingSlope(false);

        if (playerCrouch.IsCrouching && !jumpWhileCrouching) return;

        if (JumpPressed && jumpCooldownTimer <= 0 && playerGroundHandler.IsGrounded) shouldJump = true;
    }

    private void JumpFixedUpdateLogic()
    {
        if (shouldJump)
        {
            playerCrouch.StopCrouch();

            Jump();
            jumpCooldownTimer = jumpCooldown;
            shouldJump = false;
        }

        if (!playerGroundHandler.IsGrounded && !playerGroundHandler.OnSlope)
        {
            BetterJump();
        }
    }

    private void Jump()
    {
        playerGroundHandler.SetExitingSlope(true);

        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, jumpForce, _rigidBody.velocity.z);
    }

    private void BetterJump()
    {
        if (_rigidBody.velocity.y < 0)
        {
            _rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rigidBody.velocity.y > 0 && !shouldJump)
        {
            _rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
