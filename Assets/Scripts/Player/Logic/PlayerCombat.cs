using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.InputSystem.LowLevel;

public class PlayerCombat : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Settings")]
    [SerializeField] private int killCount;

    public int KillCount => killCount;

    public event EventHandler<OnKillCountEventArgs> OnKillCountSet;
    public event EventHandler<OnKillCountEventArgs> OnKillCountChanged;

    public static event EventHandler<OnKillCountEventArgs> OnLocalInstanceKillCountSet;
    public static event EventHandler<OnKillCountEventArgs> OnLocalInstanceKillCountChanged;

    public class OnKillCountEventArgs : EventArgs
    {
        public int killCount;
    }

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
        playerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }

    [PunRPC]
    private void SetKillCount(int killCount)
    {
        this.killCount = killCount;

        OnKillCountSet?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });

        if (PhotonViewMine()) OnLocalInstanceKillCountSet?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });
    }

    [PunRPC]
    private void IncreaseHealth(int quantity)
    {
        killCount += quantity;
        OnKillCountChanged?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });

        if (PhotonViewMine()) OnLocalInstanceKillCountChanged?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public int GetPhotonViewID() => photonView.ViewID;

    private void Respawn()
    {
        transform.position = GameController.Instance.PlayerSpawnPosition.position;
    }

    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Respawn();
    }

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetKillCount(killCount);
    }
}
