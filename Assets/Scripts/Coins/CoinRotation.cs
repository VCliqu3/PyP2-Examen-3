using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 initialRotation;
    [SerializeField, Range (50f,250f)] private float rotationSpeed;

    private void Start()
    {
        SetRotation(initialRotation);
    }

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 newRotation = transform.rotation.eulerAngles + new Vector3(0f,0f,rotationSpeed*Time.deltaTime);
        SetRotation(newRotation);
    }

    private void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
