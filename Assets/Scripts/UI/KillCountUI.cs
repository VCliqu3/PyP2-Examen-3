using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountUI : MonoBehaviour
{
    [Header("Comonents")]
    [SerializeField] private TextMeshProUGUI killCountText;

    private void OnEnable()
    {
        PlayerCombat.OnLocalInstanceKillCountSet += PlayerCombat_OnLocalInstanceKillCountSet;
        PlayerCombat.OnLocalInstanceKillCountChanged += PlayerCombat_OnLocalInstanceKillCountChanged;

    }
    private void OnDisable()
    {
        PlayerCombat.OnLocalInstanceKillCountSet -= PlayerCombat_OnLocalInstanceKillCountSet;
        PlayerCombat.OnLocalInstanceKillCountChanged -= PlayerCombat_OnLocalInstanceKillCountChanged;
    }

    private void SetKillCountText(int killCount)
    {
        killCountText.text = $"Kill Count: {killCount}";
    }

    private void PlayerCombat_OnLocalInstanceKillCountSet(object sender, PlayerCombat.OnKillCountEventArgs e)
    {
        SetKillCountText(e.killCount);
    }

    private void PlayerCombat_OnLocalInstanceKillCountChanged(object sender, PlayerCombat.OnKillCountEventArgs e)
    {
        SetKillCountText(e.killCount);
    }
}
