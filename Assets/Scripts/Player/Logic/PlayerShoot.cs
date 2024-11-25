using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Settings")]
    [SerializeField, Range(1f, 10f)] private float damage;
    [SerializeField, Range(1f, 5f)] private float fireRate;
    [Space]
    [SerializeField] private BulletColor bulletColor;

    private float shootTimer;

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleShooting();
        HandleShootCooldown();
    }

    private void InitializeVariables()
    {
        shootTimer = 0f;
    }

    private bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;

    private void HandleShooting()
    {
        if (!CanProcessActions()) return;
        if (!Input.GetMouseButton(0)) return;
        if (ShootOnCooldown()) return;

        FireBullet();
        ResetTimer();
    }

    private void FireBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
        
        bullet.GetComponent<BulletConnectionHandler>().SetConnection(photonView.ViewID, damage, GetShootDirection(), bulletColor);

    }

    private Vector3 GetShootDirection()
    {
        Vector3 direction = (firePoint.position - transform.position);
        direction.y = 0;

        direction.Normalize();

        return direction;
    }

    private void HandleShootCooldown()
    {
        if (!playerConnectionHandler.CanProcessActions()) return;
        if (shootTimer < 0) return;

        shootTimer -= Time.deltaTime;
    }

    private bool ShootOnCooldown() => shootTimer > 0f;
    private void ResetTimer() => shootTimer = 1f / fireRate;

    private void SetDamage(float damage) => this.damage = damage;
    private void SetBulletColor(BulletColor bulletColor) => this.bulletColor = bulletColor;

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetDamage(e.playerInfo.playerDamage);
        SetBulletColor(e.playerInfo.bulletColor);
    }
}
