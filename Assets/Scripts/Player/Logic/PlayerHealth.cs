using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerHealth : MonoBehaviourPun
{
    [Header("Settings")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField, Range(50f,100f)] private int maxHealth;
    [SerializeField] private int health;

    public event EventHandler<OnHeathEventArgs> OnHealthSet;
    public event EventHandler<OnHeathEventArgs> OnHealthChanged;

    public static event EventHandler<OnHeathEventArgs> OnLocalInstanceHealthSet;
    public static event EventHandler<OnHeathEventArgs> OnLocalInstanceHealthChanged;

    public static event EventHandler<OnPlayerKilledByPlayerEventArgs> OnPlayerKilledByPlayer;

    public event EventHandler OnPlayerDeath;

    public class OnHeathEventArgs : EventArgs
    {
        public int health;
        public int maxHealth;
    }

    public class OnPlayerKilledByPlayerEventArgs : EventArgs
    {
        public int killedPlayerID;
        public int killerPlayerID;
    }

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
        InitializeHealth();
    }

    private void InitializeHealth()
    {
        health = maxHealth;
    }

    [PunRPC]
    private void SetHealth(int health)
    {
        this.health = health;

        OnHealthSet?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });

        if (PhotonViewMine()) OnLocalInstanceHealthSet?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });
    }

    [PunRPC]
    private void IncreaseHealth(int quantity)
    {
        health = health + quantity > maxHealth? maxHealth : health + quantity;
        OnHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });  

        if(PhotonViewMine()) OnLocalInstanceHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });
    }

    [PunRPC]
    private void DecreaseHealth(int quantity)
    {
        health = health - quantity < 0 ? 0 : health - quantity;
        OnHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });

        if (PhotonViewMine()) OnLocalInstanceHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });

        if (health <= 0f)
        {
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    [PunRPC]
    private void DecreaseHealthBullet(int quantity, int shooterID)
    {
        health = health - quantity < 0 ? 0 : health - quantity;
        OnHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });

        if (PhotonViewMine()) OnLocalInstanceHealthChanged?.Invoke(this, new OnHeathEventArgs { health = health, maxHealth = maxHealth });

        if(health <= 0f)
        {
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            OnPlayerKilledByPlayer?.Invoke(this, new OnPlayerKilledByPlayerEventArgs { killedPlayerID = GetPhotonViewID(), killerPlayerID = shooterID});
        }
    }

    public void TakeDamageBullet(int damage, int shooterID)
    {
        photonView.RPC("DecreaseHealthBullet", RpcTarget.AllBuffered, damage, shooterID);
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public int GetPhotonViewID() => photonView.ViewID;

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetHealth(health);
    }
}
