using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointPositioning : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float distanceFromPlayer;

    private void Start()
    {
        SetDistanceFromPlayer();
    }

    private void Update()
    {
        HandleFirePointPosition();
    }

    private void SetDistanceFromPlayer() => distanceFromPlayer = Vector3.Distance(transform.position, player.position);

    private void HandleFirePointPosition()
    {
        transform.localPosition = new Vector3(distanceFromPlayer * GetFirePointDirection().x, transform.localPosition.y, distanceFromPlayer * GetFirePointDirection().z);
    }

    private Vector3 GetFirePointDirection()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (worldMousePos - player.position);
        direction.y = 0f;

        direction.Normalize();

        return direction;
    }
}