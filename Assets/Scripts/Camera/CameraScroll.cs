using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraScroll : MonoBehaviourPun
{
    [SerializeField] private CameraInput cameraInput;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField, Range(0f, 1f)] private float startingDistancePercent;
    [SerializeField, Range(0.01f, 0.05f)] private float smoothScrollSpeed;
    [SerializeField] private bool invertScroll;
    public float Distance { get; private set; }

    private float ScrollInput => cameraInput.GetMouseScroll();

    private float desiredDistance;

    void Start()
    {
        InitializeSettings();
    }

    void Update()
    {
        //if (!CanProcessActions()) return;

        HandleDistance();
    }

    private void InitializeSettings()
    {
        desiredDistance = startingDistancePercent * maxDistance;
        Distance = desiredDistance;
    }

    private void HandleDistance()
    {
        //Handle Inversion
        float processedScrollInput = invertScroll ? -ScrollInput : ScrollInput;

        //Set Distance
        desiredDistance = Mathf.Clamp(desiredDistance - scrollSensitivity * processedScrollInput, minDistance, maxDistance);

        Distance = Mathf.Lerp(Distance, desiredDistance, smoothScrollSpeed);
    }

    private bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
