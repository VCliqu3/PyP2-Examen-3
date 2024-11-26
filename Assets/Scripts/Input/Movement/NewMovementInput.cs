using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public override bool CanProcessMovementInput() => true;

    public override Vector2 GetMovementVectorNormalized()
    {
        if (!CanProcessMovementInput()) return Vector2.zero;

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public override bool GetJumpDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool jumpInput = playerInputActions.Player.Jump.WasPerformedThisFrame();
        return jumpInput;
    }

    public override bool GetSprintHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool runInput = playerInputActions.Player.Sprint.IsPressed();
        return runInput;
    }

    public override bool GetCrouchDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Player.Crouch.WasPerformedThisFrame();
        return crouchInput;
    }

    public override bool GetCrouchUp()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Player.Crouch.WasReleasedThisFrame();
        return crouchInput;
    }

    public override bool GetCrouchHold()
    {
        if (!CanProcessMovementInput()) return false;

        bool crouchInput = playerInputActions.Player.Crouch.IsPressed();
        return crouchInput;
    }

    public override bool GetDashDown()
    {
        if (!CanProcessMovementInput()) return false;

        bool dashInput = playerInputActions.Player.Dash.WasPerformedThisFrame();
        return dashInput;
    }
}
