using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealthBarHandler : MonoBehaviourPun
{
    [Header("Comonents")]
    [SerializeField] private PlayerConnectionHandler playerConnectionHandler;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform healthBarBG;
    [SerializeField] private Image healthBarImage;

    [Header("Settings")]
    [SerializeField] private bool hideOwn;

    private void OnEnable()
    {
        playerHealth.OnHealthSet += PlayerHealth_OnHealthSet;
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthSet -= PlayerHealth_OnHealthSet;
        playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
    }

    private void Start()
    {
        HandleHideOwn();
    }

    private void HandleHideOwn()
    {
        if (hideOwn && playerConnectionHandler.PhotonViewMine())
        {
            healthBarBG.gameObject.SetActive(false);
        }
    }

    private void SetHealthBar(int health, int maxHealh)
    {
        float fillAmount = (float)health / maxHealh;
        healthBarImage.fillAmount = fillAmount;
    }

    private void PlayerHealth_OnHealthSet(object sender, PlayerHealth.OnHeathEventArgs e)
    {
        SetHealthBar(e.health, e.maxHealth);
    }
    private void PlayerHealth_OnHealthChanged(object sender, PlayerHealth.OnHeathEventArgs e)
    {
        SetHealthBar(e.health, e.maxHealth);
    }
}
