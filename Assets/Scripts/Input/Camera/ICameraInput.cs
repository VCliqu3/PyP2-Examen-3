using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraInput
{
    public bool CanProcessCameraInput();
    public Vector2 GetLook();

    public float GetMouseScroll();
}
