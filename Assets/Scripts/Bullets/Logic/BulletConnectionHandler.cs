using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletConnectionHandler : MonoBehaviourPun
{
    public event EventHandler<OnConnectionEventArgs> OnConnection;

    public class OnConnectionEventArgs : EventArgs
    {
        public BulletInfo bulletInfo;
    }

    public void SetConnection(int ownerID, float damage, Vector3 direction, BulletColor bulletColor)
    {
        if (!PhotonViewMine()) return;

        photonView.RPC("BulletConnection", RpcTarget.AllBuffered, ownerID, damage, direction, bulletColor);
    }

    [PunRPC]
    private void BulletConnection(int ownerID, float damage, Vector3 direction, BulletColor bulletColor)
    {
        BulletInfo bulletInfo = new BulletInfo { ownerID = ownerID, damage = damage, direction = direction, bulletColor = bulletColor };

        OnConnection?.Invoke(this, new OnConnectionEventArgs { bulletInfo = bulletInfo });
    }

    public bool PhotonViewMine() => photonView.IsMine;
    public bool CanProcessActions() => photonView.IsMine && PhotonNetwork.IsConnected;
}
