using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCrouch : MonoBehaviourPun
{
    [SerializeField] private PlayerGroundHandler playerGroundHandler;

    [Header("Crouching")]
    [SerializeField] public float crouchSpeed;
    [SerializeField] float crouchYScale;

    public bool IsCrouching { get; private set; }  
    private bool CrouchHold => NewMovementInput.Instance.GetCrouchHold();
    private bool CrouchPressed => NewMovementInput.Instance.GetCrouchDown();
    private bool CrouchReleased => NewMovementInput.Instance.GetCrouchUp();

    private Rigidbody _rigidbody;
    private float startYScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startYScale = transform.localScale.y;
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
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        _rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        IsCrouching = true;
    }

    public void StopCrouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        IsCrouching = false;
    }

    public float GetCrouchSpeed() => crouchSpeed;

    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
