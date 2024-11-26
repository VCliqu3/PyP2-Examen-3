using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCameraHandler : MonoBehaviourPun
{
    public static PlayerCameraHandler LocalInstance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform cameraFollowPoint;
    [SerializeField] private Transform orientation;

    public Transform CameraFollowPoint => cameraFollowPoint;
    public Transform Orientation => orientation;

    private void Awake()
    {
        SetLocalInstance();
    }

    private void SetLocalInstance()
    {
        if (!PhotonViewMine()) return;

        LocalInstance = this;
    }

    public void SetOrientationRotation(Quaternion rotation)
    {
        orientation.rotation = rotation;
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
