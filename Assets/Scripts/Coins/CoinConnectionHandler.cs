using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CoinConnectionHandler : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private CoinController coinController;

    public event EventHandler<OnConnectionEventArgs> OnConnection;
    public event EventHandler<OnCollectionEventArgs> OnCollection;

    public class OnConnectionEventArgs : EventArgs
    {
        public CoinInfo coinInfo;
    }

    public class OnCollectionEventArgs : EventArgs
    {
        public CoinInfo coinInfo;
    }

    private void Start()
    {
        SetConnection();
    }

    #region Connection
    public void SetConnection()
    {
        if (!PhotonViewMine()) return;

        photonView.RPC("CoinConnection", RpcTarget.AllBuffered, coinController.Score, false);
    }

    [PunRPC]
    private void CoinConnection(int score, bool collected)
    {
        CoinInfo coinInfo = new CoinInfo { score = score, collected = collected };

        OnConnection?.Invoke(this, new OnConnectionEventArgs { coinInfo = coinInfo });
    }
    #endregion

    #region Collection

    public void SetCollection()
    {
        if(!PhotonViewMine()) return;

        photonView.RPC("CoinCollection", RpcTarget.AllBuffered, coinController.Score, true);
    }

    [PunRPC]
    private void CoinCollection(int score, bool collected)
    {
        CoinInfo coinInfo = new CoinInfo { score = score, collected = collected };

        OnCollection?.Invoke(this, new OnCollectionEventArgs { coinInfo = coinInfo });
    }
    #endregion

    public bool PhotonViewMine() => photonView.IsMine;
    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
