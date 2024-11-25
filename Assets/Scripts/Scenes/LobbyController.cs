using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [Header("UI Components")]
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_Dropdown playerColorDropdown;
    [SerializeField] private TMP_Dropdown bulletColorDropdown;
    [SerializeField] private Slider playerSpeedSlider;
    [SerializeField] private Slider playerDamageSlider;

    [Header("Settings")]
    [SerializeField] private string nextSceneName;

    private const int MAX_PLAYERS = 4;

    private void Awake()
    {
        InitializePhoton();
        InitializeButtonsListeners();
    }

    private void InitializePhoton() => PhotonNetwork.AutomaticallySyncScene = true;

    private void InitializeButtonsListeners()
    {
        startButton.onClick.AddListener(DoOnStartButtonClicked);
    }

    private void DoOnStartButtonClicked()
    {
        SaveDataOnUI();
        ConnectPhoton();
    }

    private void SaveDataOnUI()
    {
        GameData.playerName = playerNameInputField.text;
        GameData.playerColor = Utilities.GetPlayerColorByName(playerColorDropdown.options[playerColorDropdown.value].text);
        GameData.bulletColor = Utilities.GetBulletColorByName(bulletColorDropdown.options[bulletColorDropdown.value].text);
        GameData.playerSpeed = playerSpeedSlider.value;
        GameData.playerDamage = playerDamageSlider.value;
    }
    

    private void ConnectPhoton() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        CreateRoom();
    }

    private static void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = MAX_PLAYERS;

        PhotonNetwork.JoinOrCreateRoom("Room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        TryLoadNextScene();
    }

    private void TryLoadNextScene()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.LoadLevel(nextSceneName);
    }
}
