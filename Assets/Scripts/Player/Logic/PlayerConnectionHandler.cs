using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerConnectionHandler : MonoBehaviourPun
{
    public static GameObject LocalInstance { get; private set; }

    public event EventHandler<OnConnectionEventArgs> OnConnection;

    public static event EventHandler OnLocalInstanceConnection;
    public class OnConnectionEventArgs : EventArgs
    {
        public PlayerInfo playerInfo;
    }

    private void Awake()
    {
        SetLocalInstance();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetConnection();
    }

    private void SetLocalInstance()
    {
        if (!PhotonViewMine()) return;

        LocalInstance = gameObject;
    }

    private void SetConnection()
    {
        if (!PhotonViewMine()) return;

        photonView.RPC("PlayerConnection", RpcTarget.AllBuffered, GameData.playerName, GameData.playerColor, GameData.bulletColor, GameData.playerSpeed, GameData.playerDamage, GameData.playerFireRate);
    }

    [PunRPC]
    private void PlayerConnection(string playerName, PlayerColor playerColor, BulletColor bulletColor, float playerSpeed, float playerDamage, float playerFireRate)
    {
        PlayerInfo playerInfo = new PlayerInfo { playerName = playerName, playerColor = playerColor, bulletColor = bulletColor, playerSpeed = playerSpeed, playerDamage = playerDamage, playerFireRate = playerFireRate};

        OnConnection?.Invoke(this, new OnConnectionEventArgs { playerInfo = playerInfo});
        OnLocalInstanceConnection?.Invoke(this, EventArgs.Empty);
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}


