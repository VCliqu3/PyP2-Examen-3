using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameHandler : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private TextMeshPro playerNameText;

    [Header("Settings")]
    [SerializeField] private bool disableOwn;

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
    }

    private void SetName(string playerName) => playerNameText.text = playerName;

    private void Start()
    {
        HandleDisableOwn();
    }

    private void HandleDisableOwn()
    {
        if(disableOwn && playerConnectionHandler.PhotonViewMine())
        {
            playerNameText.gameObject.SetActive(false);
        }
    }

    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetName(e.playerInfo.playerName);
    }

}
