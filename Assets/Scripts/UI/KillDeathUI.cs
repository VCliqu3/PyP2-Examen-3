using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillDeathUI : MonoBehaviour
{
    [Header("Comonents")]
    [SerializeField] private TextMeshProUGUI killCountText;

    private int previousDeathCount;
    private int previousKillCount;

    private void OnEnable()
    {
        PlayerCombat.OnLocalInstanceKillCountSet += PlayerCombat_OnLocalInstanceKillCountSet;
        PlayerCombat.OnLocalInstanceKillCountChanged += PlayerCombat_OnLocalInstanceKillCountChanged;

        PlayerCombat.OnLocalInstanceDeathCountSet += PlayerCombat_OnLocalInstanceDeathCountSet;
        PlayerCombat.OnLocalInstanceDeathCountChanged += PlayerCombat_OnLocalInstanceDeathCountChanged;
    }

    private void OnDisable()
    {
        PlayerCombat.OnLocalInstanceKillCountSet -= PlayerCombat_OnLocalInstanceKillCountSet;
        PlayerCombat.OnLocalInstanceKillCountChanged -= PlayerCombat_OnLocalInstanceKillCountChanged;

        PlayerCombat.OnLocalInstanceDeathCountSet -= PlayerCombat_OnLocalInstanceDeathCountSet;
        PlayerCombat.OnLocalInstanceDeathCountChanged -= PlayerCombat_OnLocalInstanceDeathCountChanged;
    }

    private void UpdateKillsDeathsText()
    {
        killCountText.text = $"KD: {previousKillCount}/{previousDeathCount}";
    }

    private void PlayerCombat_OnLocalInstanceKillCountSet(object sender, PlayerCombat.OnKillCountEventArgs e)
    {
        previousKillCount = e.killCount;
        UpdateKillsDeathsText();
    }

    private void PlayerCombat_OnLocalInstanceKillCountChanged(object sender, PlayerCombat.OnKillCountEventArgs e)
    {
        previousKillCount = e.killCount;
        UpdateKillsDeathsText();
    }

    private void PlayerCombat_OnLocalInstanceDeathCountSet(object sender, PlayerCombat.OnDeathCountEventArgs e)
    {
        previousDeathCount = e.deathCount;
        UpdateKillsDeathsText();
    }

    private void PlayerCombat_OnLocalInstanceDeathCountChanged(object sender, PlayerCombat.OnDeathCountEventArgs e)
    {
        previousDeathCount = e.deathCount;
        UpdateKillsDeathsText();
    }
}
