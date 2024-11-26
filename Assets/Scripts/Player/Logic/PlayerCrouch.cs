using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerCrouch : MonoBehaviourPun
{
    [SerializeField] private PlayerGroundHandler playerGroundHandler;

    [Header("Components")]
    [SerializeField] private Transform visual;
    [SerializeField] private CapsuleCollider capsulleCollider;

    [Header("Crouching")]
    [SerializeField] public float crouchSpeed;
    [SerializeField] float crouchYScale;

    public bool IsCrouching { get; private set; }  
    private bool CrouchHold => NewMovementInput.Instance.GetCrouchHold();
    private bool CrouchPressed => NewMovementInput.Instance.GetCrouchDown();
    private bool CrouchReleased => NewMovementInput.Instance.GetCrouchUp();

    private Rigidbody _rigidbody;
    private float startYScale;
    private float startCapsulleColliderCenter;
    private float startCapsulleColliderHeight;

    public static event EventHandler OnPlayerCrouch;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startYScale = transform.localScale.y;
        startCapsulleColliderCenter = capsulleCollider.center.y;
        startCapsulleColliderHeight = capsulleCollider.height;
    }

    private void Update()
    {
        if(!CanProcessActions()) return;

        CrouchLogic();
    }

    private void CrouchLogic()
    {
        if (CrouchPressed && !IsCrouching)
        {
            StartCrouch();
        }
        else if (CrouchReleased && IsCrouching)
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        visual.localScale = new Vector3(transform.localScale.x, startYScale * crouchYScale, transform.localScale.z);
        capsulleCollider.center = new Vector3(capsulleCollider.center.x, startCapsulleColliderCenter * crouchYScale, capsulleCollider.center.z);
        capsulleCollider.height = startCapsulleColliderHeight * crouchYScale;

        _rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        IsCrouching = true;

        OnPlayerCrouch?.Invoke(this, EventArgs.Empty);
    }

    public void StopCrouch()
    {
        visual.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        capsulleCollider.center = new Vector3(capsulleCollider.center.x, startCapsulleColliderCenter, capsulleCollider.center.z);
        capsulleCollider.height = startCapsulleColliderHeight;

        IsCrouching = false;
    }

    public float GetCrouchSpeed() => crouchSpeed;

    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
