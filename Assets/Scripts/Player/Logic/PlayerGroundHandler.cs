using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundHandler : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    [Header("Slope Movement")]
    [SerializeField] public float maxSlopeAngle;
    public bool IsGrounded { get; private set; }
    public bool OnSlope { get; private set; }
    public bool ExitingSlope { get; private set; }

    private RaycastHit slopeHit; 

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        IsGrounded = GetGrounded();
        OnSlope = GetSlope();
    }

    private bool GetGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + .15f, groundMask);
    }

    private bool GetSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, _collider.bounds.extents.y + .3f, groundMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public RaycastHit GetSlopeHit() => slopeHit;
    public bool SetExitingSlope(bool boolean) => ExitingSlope = boolean;

}
