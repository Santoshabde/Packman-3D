using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAndLevelManager : SerializeSingleton<ScoreAndLevelManager>
{
    [SerializeField, ReadOnly] private int score;
    [SerializeField, ReadOnly] private int totalLivesLeft;
    [SerializeField] private int scoreIncreasePerCoin;

    public int totalScoreToCompleteLevel;

    public int Score => score;
    public int TotalScoreToCompleteLevel => totalScoreToCompleteLevel;

    public static Action<int> OnScoreUpdate;
    public static Action<int> OnLivesUpdate;
    public static Action OnGameOver;
    public static Action OnGameWin;

    private void Awake()
    {
        PackmanCollisionsController.OnCoinCollected += OnCoinCollected;
        PackmanCollisionsController.OnPackmanDeath += OnPackManDeath;

        totalLivesLeft = 3;
    }

    private void OnCoinCollected()
    {
        score += scoreIncreasePerCoin;
        OnScoreUpdate?.Invoke(score);
        if(score >= totalScoreToCompleteLevel)
        {
            OnGameWin?.Invoke();
            UIManager.Instance.OpenDialog<GameWinDialog>();
        }
    }

    private void OnPackManDeath()
    {
        totalLivesLeft -= 1;
        OnLivesUpdate(totalLivesLeft);

        if (totalLivesLeft < 1)
        {
            OnGameOver?.Invoke();
            UIManager.Instance.OpenDialog<GameloseDialog>();

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopAllAudios();
                AudioManager.Instance.PlaySound("PackmanDeath");
            }
        }
        else
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("PackmanDeath", true);
            }
        }
    }

    public void SetTotalScoreToCompleteLevel(int totalCoins)
    {
        totalScoreToCompleteLevel = totalCoins * scoreIncreasePerCoin;
    }
}
