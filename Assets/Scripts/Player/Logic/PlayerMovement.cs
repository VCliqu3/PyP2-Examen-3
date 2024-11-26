using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private PlayerGroundHandler playerGroundHandler;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerCrouch playerCrouch;

    [SerializeField] private Transform orientation;
    private Rigidbody _rigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float currentMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintSpeedMultiplier;

    [Header("Friction & Resistance")]
    [SerializeField] private float groundFriction;
    [SerializeField] private float airResistance;

    [Header("Speed Smoothing")]
    [SerializeField] private float smoothingMultiplier;
    [SerializeField] private float minimalSmoothingDifference;

    [Header("Movement State")]
    [HideInInspector] public MovementState movementState;

    private float desiredMovementSpeed;
    private float lastDesiredMaxSpeed;
    private IEnumerator smoothingCoroutine;

    private Vector2 MovementInput => NewMovementInput.Instance.GetMovementVectorNormalized();
    private bool SprintHold => NewMovementInput.Instance.GetSprintHold();

    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Air,
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!CanProcessActions()) return;

        DefineMovementSpeed();
        SmoothSpeed();
    }
    private void FixedUpdate()
    {
        if (!CanProcessActions()) return;

        Movement();
        SpeedControl();
        Friction();
        AirResistance();
    }

    private void DefineMovementSpeed()
    {
        if (!playerGroundHandler.IsGrounded)
        {
            SetMovementState(MovementState.Air);
            return;
        }

        if (MovementInput == Vector2.zero)
        {
            SetMovementState(MovementState.Idle);
            desiredMovementSpeed = 0f;
            return;
        }

        if (playerCrouch.IsCrouching)
        {
            SetMovementState(MovementState.Crouching);
            desiredMovementSpeed = playerCrouch.GetCrouchSpeed();
            return;
        }

        if (SprintHold)
        {
            SetMovementState(MovementState.Sprinting);
            desiredMovementSpeed = sprintSpeed;
            return;
        }

        SetMovementState(MovementState.Walking);
        desiredMovementSpeed = walkSpeed;
    }

    private void SetMovementState(MovementState state) => movementState = state;

    private void SmoothSpeed()
    {
        if (Mathf.Abs(desiredMovementSpeed - lastDesiredMaxSpeed) > minimalSmoothingDifference && currentMovementSpeed != 0 && !playerDash.IsDashing)
        {
            if (smoothingCoroutine != null) StopCoroutine(smoothingCoroutine);
            smoothingCoroutine = SmoothlyLerpSpeed();
            StartCoroutine(smoothingCoroutine);
        }
        else if (desiredMovementSpeed != lastDesiredMaxSpeed)
        {
            currentMovementSpeed = desiredMovementSpeed;
        }

        lastDesiredMaxSpeed = desiredMovementSpeed;
    }

    private IEnumerator SmoothlyLerpSpeed()
    {
        float time = 0;
        float diff = Mathf.Abs(desiredMovementSpeed - currentMovementSpeed);
        float startValue = currentMovementSpeed;

        while (time < diff)
        {
            currentMovementSpeed = Mathf.Lerp(startValue, desiredMovementSpeed, time / diff);
            time += Time.deltaTime * smoothingMultiplier;
            yield return null;
        }

        currentMovementSpeed = desiredMovementSpeed;
    }

    private void Movement()
    {
        if (playerDash.IsDashing) return;

        Vector3 moveDirection = MovementInput.x * orientation.right + MovementInput.y * orientation.forward;

        if (playerGroundHandler.OnSlope)
        {
            _rigidbody.AddForce(GetSlopeVector(moveDirection).normalized * acceleration * 1.5f);

            if (_rigidbody.velocity.y > 0)
                _rigidbody.AddForce(Vector3.down * 80f);
        }
        else
        {
            _rigidbody.AddForce(moveDirection.normalized * acceleration);
        }
    }

    private void SpeedControl()
    {
        if (playerDash.IsDashing) return;

        if (playerGroundHandler.OnSlope && !playerGroundHandler.ExitingSlope)
        {
            if (_rigidbody.velocity.magnitude > currentMovementSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * currentMovementSpeed;
            }
        }
        else
        {
            Vector3 limitedVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            if (limitedVelocity.magnitude > currentMovementSpeed)
            {
                limitedVelocity = limitedVelocity.normalized * currentMovementSpeed;
                _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Friction()
    {
        if (playerDash.IsDashing) return;

        if (playerGroundHandler.IsGrounded)
        {
            if (MovementInput.x == 0f || ChangingDirectionsX)
            {
                _rigidbody.AddForce(Vector3.left * _rigidbody.velocity.x * groundFriction);
            }

            if (MovementInput.y == 0f || ChangingDirectionsZ)
            {
                _rigidbody.AddForce(Vector3.back * _rigidbody.velocity.z * groundFriction);
            }
        }

    }

    private void AirResistance()
    {
        if (playerDash.IsDashing) return;

        if (!playerGroundHandler.IsGrounded)
        {
            _rigidbody.AddForce(Vector3.left * _rigidbody.velocity.x * airResistance);
            _rigidbody.AddForce(Vector3.back * _rigidbody.velocity.z * airResistance);
        }
    }

    private Vector3 GetSlopeVector(Vector3 vec) => Vector3.ProjectOnPlane(vec, playerGroundHandler.GetSlopeHit().normal);
    private bool ChangingDirectionsX => Mathf.Sign(Vector3.Dot(_rigidbody.velocity, orientation.right)) != Mathf.Sign(MovementInput.x) && MovementInput.x != 0f;
    private bool ChangingDirectionsZ => Mathf.Sign(Vector3.Dot(_rigidbody.velocity, orientation.forward)) != Mathf.Sign(MovementInput.y) && MovementInput.y != 0f;

    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;

    private void SetSpeed(float speed)
    {
        sprintSpeed = speed;
        walkSpeed = speed / sprintSpeedMultiplier;
    }

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetSpeed(e.playerInfo.playerSpeed);
    }
}