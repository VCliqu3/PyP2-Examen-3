using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    public static GameController Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Settings")]
    [SerializeField] private Transform playerSpawnPosition;

    public Transform PlayerSpawnPosition => playerSpawnPosition;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        TryCreatePlayerOnStart();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        TryCreatePlayerOnJoinedRoom();
    }

    private void TryCreatePlayerOnStart()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        if (PlayerConnectionHandler.LocalInstance != null) return;

        PhotonNetwork.Instantiate(playerPrefab.name, playerSpawnPosition.position, Quaternion.identity);     
    }

    private void TryCreatePlayerOnJoinedRoom()
    {
        if (PlayerConnectionHandler.LocalInstance != null) return;
        
        PhotonNetwork.Instantiate(playerPrefab.name, playerSpawnPosition.position, Quaternion.identity);
    }
}