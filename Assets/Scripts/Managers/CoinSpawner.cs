using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private List<Transform> coinPositions;

    private void Start()
    {
        TryCreateCoinsOnStart();
    }
    public override void OnJoinedRoom()
    {
        TryCreateCoinsOnJoinedRoom();
    }

    private void TryCreateCoinsOnStart()
    {
        foreach(Transform coinPosition in coinPositions)
        {
            PhotonNetwork.Instantiate(coinPrefab.name, coinPosition.position, Quaternion.identity);
        }
    }

    private void TryCreateCoinsOnJoinedRoom()
    {
        if (PlayerConnectionHandler.LocalInstance != null) return;

        foreach (Transform coinPosition in coinPositions)
        {
            PhotonNetwork.Instantiate(coinPrefab.name, coinPosition.position, Quaternion.identity);
        }
    }
}
