using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraInput : MonoBehaviour, ICameraInput
{
    public static CameraInput Instance { get; private set; }

    protected virtual void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one CameraInput instance");
        }

        Instance = this;
    }

    public abstract bool CanProcessCameraInput();

    public abstract Vector2 GetLook();

    public abstract float GetMouseScroll();
}
