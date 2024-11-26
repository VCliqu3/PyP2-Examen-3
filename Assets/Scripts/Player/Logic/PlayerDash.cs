using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDash : MonoBehaviourPun
{
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerGroundHandler playerGroundHandler;
    [SerializeField] private Transform orientation;

    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashMaxSpeed;
    [SerializeField] private float dashResistance;

    [SerializeField] private bool dashWhileCrouching;
    [SerializeField] private bool disableGravity;
    [SerializeField] private bool oneDashPerGrounded;

    private float dashCooldownTimer;
    private float dashPerformTimer;
    private bool hasTouchedGround;
    public bool IsDashing { get; private set; }

    private Rigidbody _rigidbody;

    private Vector2 MovementInput => NewMovementInput.Instance.GetMovementVectorNormalized();
    private bool DashPressed => NewMovementInput.Instance.GetDashDown();
    private bool shouldDash;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!CanProcessActions()) return;

        DashUpdateLogic();
        CheckTouchedGround();
    }

    private void FixedUpdate()
    {
        if(!CanProcessActions()) return;

        DashFixedUpdateLogic();
        DashResistance();
    }

    private void DashUpdateLogic()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (IsDashing) StopDash();

        if (playerCrouch.IsCrouching && !dashWhileCrouching) return;

        if (oneDashPerGrounded && !hasTouchedGround) return;

        if (DashPressed && dashCooldownTimer <= 0) shouldDash = true;
    }
    private void DashFixedUpdateLogic()
    {
        if (shouldDash)
        {
            Dash();
            shouldDash = false;
            dashCooldownTimer = dashCooldown;
            dashPerformTimer = dashTime;
            hasTouchedGround = false;
        }
    }

    public void Dash()
    {
        if (disableGravity) _rigidbody.useGravity = false;

        Vector3 direction = MovementInput.x * orientation.right + MovementInput.y * orientation.forward;
        if (direction.sqrMagnitude == 0f) direction = orientation.forward;

        float dashForce = dashDistance / dashTime;

        _rigidbody.velocity = direction * dashForce;
        IsDashing = true;
    }

    private void StopDash()
    {
        if (disableGravity) _rigidbody.useGravity = true;

        _rigidbody.velocity = Vector3.zero;
        IsDashing = false;
    }

    private void DashResistance()
    {
        if (IsDashing)
        {
            _rigidbody.AddForce(Vector3.left * _rigidbody.velocity.x * dashResistance);
            _rigidbody.AddForce(Vector3.back * _rigidbody.velocity.z * dashResistance);
        }
    }

    private void CheckTouchedGround()
    {
        if (IsDashing) return;
        if (!playerGroundHandler.IsGrounded) return;

        hasTouchedGround = true;
    }

    public float GetDashMaxSpeed() => dashMaxSpeed;

    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
