using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreTextHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private TextMeshPro playerScoreText;

    private void OnEnable()
    {
        playerScore.OnScoreSet += PlayerScore_OnScoreSet;
    }

    private void OnDisable()
    {
        playerScore.OnScoreSet -= PlayerScore_OnScoreSet;
    }

    private void SetScoreText(int score) => playerScoreText.text = $"Score: {score}";

    private void PlayerScore_OnScoreSet(object sender, PlayerScore.OnScoreSetEventArgs e)
    {
        SetScoreText(e.score);
    }
}
