using UnityEngine;

public enum CurrentGameMode
{
    None,
    Scatter,
    Chase,
    Frightened,
    Eaten
}

[System.Serializable]
public class GameTimer
{
    public float scatterTime;
    public float chaseTime;

    public float ScatterTime => scatterTime;
    public float ChaseTime => chaseTime;

    public GameTimer()
    {
        scatterTime = 0;
        chaseTime = 0;
    }

    public void Tick(CurrentGameMode currentGameMode)
    {
        if (currentGameMode == CurrentGameMode.Scatter)
        {
            chaseTime = 0;
            scatterTime += Time.deltaTime;
        }

        else if (currentGameMode == CurrentGameMode.Chase)
        {
            scatterTime = 0;
            chaseTime += Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        scatterTime = 0;
        chaseTime = 0;
    }
}