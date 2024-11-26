using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraInput : CameraInput
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
        playerInputActions.Camera.Enable();
    }

    public override bool CanProcessCameraInput() => true;

    public override Vector2 GetLook()
    {
        if (!CanProcessCameraInput()) return Vector2.zero;

        Vector2 lookVector = playerInputActions.Camera.Look.ReadValue<Vector2>();
        //lookVector = lookVector.normalized;
        return lookVector;
    }

    public override float GetMouseScroll()
    {
        if (!CanProcessCameraInput()) return 0f;

        float scrollValue = playerInputActions.Camera.MouseScroll.ReadValue<float>();
        return scrollValue;
    }
}
