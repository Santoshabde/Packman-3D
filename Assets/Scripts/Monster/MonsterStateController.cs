using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// All Monster State changes happen through here. Any monster state change should go from here!!
/// </summary>
public class MonsterStateController : SerializeSingleton<MonsterStateController>
{
    [SerializeField] private LevelMonsterModesConfig levelMonsterModesConfig;
    [SerializeField] private int currentLevel;

    public static Action<CurrentGameMode> OnStateChange;

    private GameTimer timer;
    private CurrentGameMode currentGameMode;
    private int currentWave;
    private int configuredWaveScatterTime;
    private int configuredWaveChaseTime;

    void Start()
    {
        //Timer
        timer = new GameTimer();
        timer.ResetTimer();

        //Current Game Mode, Wave, Chase and Scatter time settings
        currentGameMode = CurrentGameMode.None;
        currentWave = 0;
        configuredWaveChaseTime = GetWaveTimes(currentLevel, currentWave).Item1;
        configuredWaveScatterTime = GetWaveTimes(currentLevel, currentWave).Item2;

        OnStateChange += OnChangeInState;

        StartCoroutine(OnGameStart());
    }
    private void Update()
    {
        //Run infinity chase mode/Scatter mode if it is -1!!
        if (configuredWaveChaseTime == -1
            && currentGameMode == CurrentGameMode.Chase)
        {
            return;
        }

        timer.Tick(currentGameMode);

        if (currentGameMode == CurrentGameMode.Scatter)
        {
            if (timer.ScatterTime > configuredWaveScatterTime && configuredWaveScatterTime != -1)
            {
                timer.ResetTimer();
                OnStateChange?.Invoke(CurrentGameMode.Chase);
            }

            else if (configuredWaveScatterTime != -1)
            {
                OnStateChange?.Invoke(CurrentGameMode.Scatter);
            }
        }

        if(currentGameMode == CurrentGameMode.Chase)
        {
            if (timer.ChaseTime > configuredWaveChaseTime && configuredWaveChaseTime != -1)
            {
                timer.ResetTimer();
                OnStateChange?.Invoke(CurrentGameMode.Scatter);

                currentWave += 1;
                configuredWaveChaseTime = GetWaveTimes(currentLevel, currentWave).Item1;
                configuredWaveScatterTime = GetWaveTimes(currentLevel, currentWave).Item2;
            }

            else if(configuredWaveChaseTime != -1)
            {
                OnStateChange?.Invoke(CurrentGameMode.Chase);
            }
        }
    }
    IEnumerator OnGameStart()
    {
        while(!LevelBuilder.startGame)
        {
            yield return null;
        }

        //Initial Mode on Game State
        OnStateChange?.Invoke(CurrentGameMode.Scatter);
    }


    #region On Packman Powerup Collected
    public void OnPackmanPowerUp()
    {
        StartCoroutine(OnPakmanPowerUpCollected());
    }

    private IEnumerator OnPakmanPowerUpCollected()
    {
        CurrentGameMode previousGameMode = currentGameMode;
        OnStateChange?.Invoke(CurrentGameMode.Frightened);

        yield return new WaitForSeconds(15f);

        if (previousGameMode == CurrentGameMode.Chase)
            OnStateChange?.Invoke(CurrentGameMode.Chase);

        else
            OnStateChange?.Invoke(CurrentGameMode.Scatter);
    }

    #endregion

    public void ChangeMonsterState(Monster monster, IMonsterState monsterState)
    {
        monster.CurrentMonsterState = monsterState;
    }

    private void OnChangeInState(CurrentGameMode gameMode)
    {
        currentGameMode = gameMode;
    }

    /// <summary>
    /// Get chase and scatter time configured for the particular level and wave!! (Chase, Scatter)
    /// </summary>
    /// <param name="level"> Current Level </param>
    /// <param name="wave"> Current Wave </param>
    /// <returns> (Chase, Scatter) times configured for this level and wave </returns>
    private (int, int) GetWaveTimes(int level, int wave)
    {
        foreach (var item in levelMonsterModesConfig.modesData)
        {
            if (currentLevel >= item.levelMin && currentLevel <= item.levelMax)
            {
                return (item.chaseScatterTimings[wave].chaseTiming, item.chaseScatterTimings[wave].scatterTiming);
            }
        }
        return (-1, -1);
    }
}
