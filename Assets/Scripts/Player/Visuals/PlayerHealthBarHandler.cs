using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarHandler : MonoBehaviour
{
    [Header("Comonents")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthBarImage;

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
