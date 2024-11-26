using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private Transform orientation;

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (!playerConnectionHandler.CanProcessActions()) return;

        transform.rotation = orientation.rotation;
    }
}
