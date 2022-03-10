using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    InProgress,
    GameWin,
    GameLose
}

public class GameStateManager : SerializeSingleton<GameStateManager>
{
    [SerializeField, ReadOnly] private GameState currentGameState;

    public GameState CurrentGameState => currentGameState;
    void Awake()
    {
        currentGameState = GameState.InProgress;

        ScoreAndLevelManager.OnGameOver += OnGameLost;
        ScoreAndLevelManager.OnGameWin += OnGameWin;
    }

    private void OnGameLost()
    {
        currentGameState = GameState.GameLose;
    }

    private void OnGameWin()
    {
        currentGameState = GameState.GameWin;
    }
}
