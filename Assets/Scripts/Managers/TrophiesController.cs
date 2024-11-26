using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJolt.API;

public class TrophiesController : MonoBehaviour
{
    private const int CONNNECTION_TROPHY_ID = 251048;
    private const int KILLS_TROPHY_1_ID = 251050;
    private const int KILLS_TROPHY_2_ID = 251051;
    private const int DEATHS_TROPHY_1_ID = 251049;
    private const int DEATHS_TROPHY_2_ID = 251052;

    private const int KILL_TROPHY_1_KILLS = 1;
    private const int KILL_TROPHY_2_KILLS = 5;
    private const int DEATH_TROPHY_1_DEATHS = 1;
    private const int DEATH_TROPHY_2_DEATHS = 5;

    private void OnEnable()
    {
        PlayerConnectionHandler.OnLocalInstanceConnection += PlayerConnectionHandler_OnLocalInsatnceConnection;
        PlayerCombat.OnLocalInstanceKillCountChanged += PlayerCombat_OnLocalInstanceKillCountChanged;
        PlayerCombat.OnLocalInstanceDeathCountChanged += PlayerCombat_OnLocalInstanceDeathCountChanged;
    }

    private void OnDisable()
    {
        PlayerConnectionHandler.OnLocalInstanceConnection -= PlayerConnectionHandler_OnLocalInsatnceConnection;
        PlayerCombat.OnLocalInstanceKillCountChanged -= PlayerCombat_OnLocalInstanceKillCountChanged;
        PlayerCombat.OnLocalInstanceDeathCountChanged -= PlayerCombat_OnLocalInstanceDeathCountChanged;
    }


    private void Awake()
    {
        Trophies.Remove(CONNNECTION_TROPHY_ID);
        Trophies.Remove(KILLS_TROPHY_1_ID);
        Trophies.Remove(KILLS_TROPHY_2_ID);
        Trophies.Remove(DEATHS_TROPHY_1_ID);
        Trophies.Remove(DEATHS_TROPHY_2_ID);
    }

    private void CheckAchieveConnectionTrophy()
    {
        Trophies.TryUnlock(CONNNECTION_TROPHY_ID);
    }

    private void CheckAchieveKills1Trophy(int kills)
    {
        if (kills < KILL_TROPHY_1_KILLS) return;

        Trophies.TryUnlock(KILLS_TROPHY_1_ID);
    }

    private void CheckAchieveKills2Trophy(int kills)
    {
        if (kills < KILL_TROPHY_2_KILLS) return;

        Trophies.TryUnlock(KILLS_TROPHY_2_ID);
    }

    private void CheckAchieveDeaths1Trophy(int deaths)
    {
        if (deaths < DEATH_TROPHY_1_DEATHS) return;

        Trophies.TryUnlock(DEATHS_TROPHY_1_ID);
    }

    private void CheckAchieveDeaths2Trophy(int deaths)
    {
        if (deaths < DEATH_TROPHY_2_DEATHS) return;

        Trophies.TryUnlock(DEATHS_TROPHY_2_ID);
    }

    private void PlayerConnectionHandler_OnLocalInsatnceConnection(object sender, System.EventArgs e)
    {
        CheckAchieveConnectionTrophy();
    }

    private void PlayerCombat_OnLocalInstanceKillCountChanged(object sender, PlayerCombat.OnKillCountEventArgs e)
    {
        CheckAchieveKills1Trophy(e.killCount);
        CheckAchieveKills2Trophy(e.killCount);
    }

    private void PlayerCombat_OnLocalInstanceDeathCountChanged(object sender, PlayerCombat.OnDeathCountEventArgs e)
    {
        CheckAchieveDeaths1Trophy(e.deathCount);
        CheckAchieveDeaths2Trophy(e.deathCount);
    }
}
