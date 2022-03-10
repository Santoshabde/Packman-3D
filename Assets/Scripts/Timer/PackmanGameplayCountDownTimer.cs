using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanGameplayCountDownTimer : CountDownTimer
{
    protected override bool StartTheClock() => LevelBuilder.startGame;

    protected override bool RunTheClock() => GameStateManager.Instance.CurrentGameState == GameState.InProgress;
}
