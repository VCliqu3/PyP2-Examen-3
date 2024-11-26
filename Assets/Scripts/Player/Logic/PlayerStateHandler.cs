using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Player Scripts")]
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private PlayerGroundHandler playerGroundHandler;
    [SerializeField] private Transform orientation;

    [Header("Movement State")]
    [SerializeField] private PlayerState playerState;

    private enum PlayerState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Dashing,
        Air,
    }

    private void Update()
    {
        HandlePlayerState();
    }

    private void HandlePlayerState()
    {
        if (playerDash.IsDashing)
        {
            SetPlayerState(PlayerState.Dashing);
            return;
        }

        if (playerCrouch.IsCrouching)
        {
            SetPlayerState(PlayerState.Crouching);
            return;
        }

        if (!playerGroundHandler.IsGrounded)
        {
            SetPlayerState(PlayerState.Air);
            return;
        }

        if (playerMovement.movementState == PlayerMovement.MovementState.Idle)
        {
            SetPlayerState(PlayerState.Idle);
            return;
        }

        if (playerMovement.movementState == PlayerMovement.MovementState.Walking)
        {
            SetPlayerState(PlayerState.Walking);
            return;
        }

        if (playerMovement.movementState == PlayerMovement.MovementState.Sprinting)
        {
            SetPlayerState(PlayerState.Sprinting);
            return;
        }
    }

    private void SetPlayerState(PlayerState state) => playerState = state;
}
