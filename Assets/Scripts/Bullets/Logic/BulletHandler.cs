using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private BulletConnectionHandler bulletConnectionHandler;

    [Header("Settings")]
    [SerializeField, Range(5f,15f)] private float speed;
    [SerializeField, Range(1f,10f)] private float damage;
    [SerializeField, Range(5f,10f)] private float lifespan;
    [SerializeField] private int ownerId;

    private Rigidbody rb;
    private Vector3 direction;

    private void OnEnable()
    {
        bulletConnectionHandler.OnConnection += BulletConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        bulletConnectionHandler.OnConnection -= BulletConnectionHandler_OnConnection;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(LifespanCoroutine());
    }

    void Update()
    {
        HandleMovement();
    }

    private bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;

    private bool PhotonViewMine() => photonView.IsMine;

    private void SetOwnerID(int ownerId)
    {
        this.ownerId = ownerId;
    }

    private void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void HandleMovement()
    {
        if (!CanProcessActions()) return;

        rb.velocity = direction * speed;
    }

    private IEnumerator LifespanCoroutine()
    {
        yield return new WaitForSeconds(lifespan);

        if (!CanProcessActions()) yield break;

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonViewMine()) return;

        if(other.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(ownerId != playerHealth.GetPhotonViewID())
            {
                playerHealth.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }

    private void BulletConnectionHandler_OnConnection(object sender, BulletConnectionHandler.OnConnectionEventArgs e)
    {
        SetOwnerID(e.bulletInfo.ownerID);
        SetDirection(e.bulletInfo.direction);
        SetDamage(e.bulletInfo.damage);
    }
}