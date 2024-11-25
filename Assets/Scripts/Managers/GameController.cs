using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Settings")]
    [SerializeField] private Vector3 playerInstantiationPosition;

    private void Start()
    {
        TryCreatePlayerOnStart();
    }
    public override void OnJoinedRoom()
    {
        TryCreatePlayerOnJoinedRoom();
    }

    private void TryCreatePlayerOnStart()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        if (PlayerConnectionHandler.LocalInstance != null) return;

        PhotonNetwork.Instantiate(playerPrefab.name,playerInstantiationPosition, Quaternion.identity);     
    }

    private void TryCreatePlayerOnJoinedRoom()
    {
        if (PlayerConnectionHandler.LocalInstance != null) return;
        
        PhotonNetwork.Instantiate(playerPrefab.name, playerInstantiationPosition, Quaternion.identity);
    }
}