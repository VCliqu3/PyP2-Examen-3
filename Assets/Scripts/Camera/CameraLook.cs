using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraLook : MonoBehaviourPun
{
    [SerializeField] private CameraInput cameraInput;
    [SerializeField] private CameraScroll cameraScroll;

    [Header("Camera Transforms")]
    [SerializeField] private Transform parent;

    [Header("Look Settings")]
    [SerializeField] private Vector2 lookSensitivity;
    [SerializeField] private float maxPolarAngleUpperLimit;
    [SerializeField] private float maxPolarAngleLowerLimit;
    [SerializeField, Range(0.01f, 0.05f)] private float smoothRotationSpeed;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    private Vector2 LookInput => cameraInput.GetLook();

    private float azimutal;
    private float polar;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitializeSettings();
    }

    private void InitializeSettings()
    {
        azimutal = 0f;
        polar = 90f;
    }

    void Update()
    {
        //if (!CanProcessActions()) return;

        HandleRotation();   
    }

    private void HandleRotation()
    {
        if (!CanHandleRotation()) return;

        //HandleInversion
        Vector2 ProcessedLookInput;
        ProcessedLookInput.x = invertX ? -LookInput.x : LookInput.x;
        ProcessedLookInput.y = invertY ? -LookInput.y : LookInput.y;

        //Calculate Angles
        azimutal = (azimutal + ProcessedLookInput.x * lookSensitivity.x) % 360;
        polar = Mathf.Clamp(polar + ProcessedLookInput.y * lookSensitivity.y, maxPolarAngleLowerLimit, maxPolarAngleUpperLimit);

        //Calculate rotation
        Quaternion targetRot = Quaternion.Euler(new Vector3(polar, azimutal, 0f));
        Quaternion actualRot = Quaternion.Slerp(transform.rotation, targetRot, smoothRotationSpeed);

        //Set Position
        parent.position = PlayerCameraHandler.LocalInstance.CameraFollowPoint.position - actualRot * new Vector3(0f, 0f, cameraScroll.Distance);

        //Set Rotation
        transform.rotation = actualRot;
        PlayerCameraHandler.LocalInstance.SetOrientationRotation(Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)));
    }

    private bool CanHandleRotation()
    {
        if (PlayerCameraHandler.LocalInstance == null) return false;
        if (!PlayerCameraHandler.LocalInstance.CanProcessActions()) return false;

        return true;
    }
    private bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
