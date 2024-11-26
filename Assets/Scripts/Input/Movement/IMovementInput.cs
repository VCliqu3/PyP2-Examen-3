using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInput
{
    public bool CanProcessMovementInput();
    public Vector2 GetMovementVectorNormalized();
    public bool GetSprintHold();
    public bool GetJumpDown();
    public bool GetCrouchDown();
    public bool GetCrouchUp();
    public bool GetCrouchHold();
    public bool GetDashDown();
}
