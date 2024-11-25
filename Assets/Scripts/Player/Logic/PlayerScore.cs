using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScore : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;

    [Header("Settings")]
    [SerializeField] private int score;

    public event EventHandler<OnScoreSetEventArgs> OnScoreSet;

    public class OnScoreSetEventArgs : EventArgs
    {
        public int score;
    }

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
    }

    [PunRPC]
    private void SetScore(int score)
    {
        this.score = score;

        OnScoreSet?.Invoke(this, new OnScoreSetEventArgs { score = score});
    }

    [PunRPC]
    private void AddScore(int quantity)
    {
        score += quantity;
        OnScoreSet?.Invoke(this, new OnScoreSetEventArgs { score = score });
    }

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetScore(score);
    }

    public bool PhotonViewMine() => photonView.IsMine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CoinController coinController))
        {
            coinController.GetComponent<CoinConnectionHandler>().SetCollection();

            if (!PhotonViewMine()) return;         
            photonView.RPC("AddScore", RpcTarget.AllBuffered, coinController.Score);
        }
    }
}
