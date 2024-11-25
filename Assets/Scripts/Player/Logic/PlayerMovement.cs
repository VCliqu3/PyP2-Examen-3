using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;

    [Header("Settings")]
    [SerializeField,Range(1f,10f)] private float speed;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
    }


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!playerConnectionHandler.CanProcessActions()) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movementVector = new Vector3(horizontal * speed, _rigidbody.velocity.y, vertical * speed);

        _rigidbody.velocity = movementVector;
    }

    private void SetSpeed(float speed) => this.speed = speed;

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetSpeed(e.playerInfo.playerSpeed);
    }
}
