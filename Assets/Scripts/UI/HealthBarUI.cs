using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Comonents")]
    [SerializeField] private Image healthBarImage;

    private void OnEnable()
    {
        PlayerHealth.OnLocalInstanceHealthSet += PlayerHealth_OnLocalInstanceHealthSet;
        PlayerHealth.OnLocalInstanceHealthChanged += PlayerHealth_OnLocalInstanceHealthChanged;

    }
    private void OnDisable()
    {
        PlayerHealth.OnLocalInstanceHealthSet -= PlayerHealth_OnLocalInstanceHealthSet;
        PlayerHealth.OnLocalInstanceHealthChanged -= PlayerHealth_OnLocalInstanceHealthChanged;
    }

    private void SetHealthBar(int health, int maxHealh)
    {
        float fillAmount = (float)health / maxHealh;
        healthBarImage.fillAmount = fillAmount;
    }

    private void PlayerHealth_OnLocalInstanceHealthSet(object sender, PlayerHealth.OnHeathEventArgs e)
    {
        SetHealthBar(e.health, e.maxHealth);
    }

    private void PlayerHealth_OnLocalInstanceHealthChanged(object sender, PlayerHealth.OnHeathEventArgs e)
    {
        SetHealthBar(e.health, e.maxHealth);
    }
}
