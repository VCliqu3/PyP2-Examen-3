using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerColorHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private List<Renderer> playerRenderers;
    [Space]
    [SerializeField] private List<PlayerMaterial> playerMaterials;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Serializable]
    public class PlayerMaterial
    {
        public PlayerColor playerColor;
        public Material material;
    }

    private void OnEnable()
    {
        playerConnectionHandler.OnConnection += PlayerConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        playerConnectionHandler.OnConnection -= PlayerConnectionHandler_OnConnection;
    }

    
    private void SetMaterialByPlayerColor(PlayerColor playerColor)
    {
        Material mat = GetMaterialByPlayerColor(playerColor);

        if (!mat)
        {
            if (debug) Debug.Log($"No material was found for PlayerColor :{playerColor}");
        }

        foreach (Renderer playerRenderer in playerRenderers)
        {
            playerRenderer.material = mat;
        }
    }

    private Material GetMaterialByPlayerColor(PlayerColor playerColor)
    {
        foreach (PlayerMaterial playerMaterial in playerMaterials)
        {
            if(playerMaterial.playerColor == playerColor)
            {
                return playerMaterial.material;
            }
        }

        return null;
    }
    
    private void PlayerConnectionHandler_OnConnection(object sender, PlayerConnectionHandler.OnConnectionEventArgs e)
    {
        SetMaterialByPlayerColor(e.playerInfo.playerColor);
    }
}
