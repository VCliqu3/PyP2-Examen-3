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
    [SerializeField] private int deathCount;

    public int KillCount => killCount;
    public int DeathCount => deathCount;    

    public event EventHandler<OnKillCountEventArgs> OnKillCountSet;
    public event EventHandler<OnKillCountEventArgs> OnKillCountChanged;

    public static event EventHandler<OnKillCountEventArgs> OnLocalInstanceKillCountSet;
    public static event EventHandler<OnKillCountEventArgs> OnLocalInstanceKillCountChanged;

    public event EventHandler<OnDeathCountEventArgs> OnDeathCountSet;
    public event EventHandler<OnDeathCountEventArgs> OnDeathCountChanged;

    public static event EventHandler<OnDeathCountEventArgs> OnLocalInstanceDeathCountSet;
    public static event EventHandler<OnDeathCountEventArgs> OnLocalInstanceDeathCountChanged;

    public event EventHandler OnPlayerRespawn;

    public class OnKillCountEventArgs : EventArgs
    {
        public int killCount;
    }

    public class OnDeathCountEventArgs : EventArgs
    {
        public int deathCount;
    }

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerHealth.OnPlayerKilledByPlayer += PlayerHealth_OnPlayerKilledByPlayer;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
        playerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        PlayerHealth.OnPlayerKilledByPlayer -= PlayerHealth_OnPlayerKilledByPlayer;
    }

    [PunRPC]
    private void SetKillCount(int killCount)
    {
        this.killCount = killCount;

        OnKillCountSet?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });

        if (PhotonViewMine()) OnLocalInstanceKillCountSet?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });
    }

    [PunRPC]
    private void IncreaseKillCount(int quantity)
    {
        killCount += quantity;
        OnKillCountChanged?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });

        if (PhotonViewMine()) OnLocalInstanceKillCountChanged?.Invoke(this, new OnKillCountEventArgs { killCount = killCount });
    }

    [PunRPC]
    private void SetDeathCount(int deathCount)
    {
        this.deathCount = deathCount;

        OnDeathCountSet?.Invoke(this, new OnDeathCountEventArgs { deathCount = deathCount });

        if (PhotonViewMine()) OnLocalInstanceDeathCountSet?.Invoke(this, new OnDeathCountEventArgs { deathCount = deathCount });
    }

    [PunRPC]
    private void IncreaseDeathCount(int deathCount)
    {
        deathCount += deathCount;
        OnDeathCountChanged?.Invoke(this, new OnDeathCountEventArgs { deathCount = deathCount });

        if (PhotonViewMine()) OnLocalInstanceDeathCountChanged?.Invoke(this, new OnDeathCountEventArgs { deathCount = deathCount });
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public int GetPhotonViewID() => photonView.ViewID;

    private void Respawn()
    {
        if (!PhotonViewMine()) return;

        transform.position = GameController.Instance.PlayerSpawnPosition.position;
        OnPlayerRespawn?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Respawn();
        photonView.RPC("IncreaseDeathCount", RpcTarget.AllBuffered, 1);
    }

    private void PlayerHealth_OnPlayerKilledByPlayer(object sender, PlayerHealth.OnPlayerKilledByPlayerEventArgs e)
    {
        if (!PhotonViewMine()) return;

        if(GetPhotonViewID() == e.killerPlayerID)
        {
           photonView.RPC("IncreaseKillCount", RpcTarget.AllBuffered, 1);
        }
    }

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetKillCount(killCount);
        SetDeathCount(deathCount);
    }
}
