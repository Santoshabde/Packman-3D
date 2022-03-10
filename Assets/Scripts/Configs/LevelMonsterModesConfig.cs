using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMonsterModesConfig", menuName = "Configs/LevelMonsterModesConfig", order = 1)]
public class LevelMonsterModesConfig : BaseConfig
{
    public List<ModesData> modesData;
}

[System.Serializable]
public class ModesData
{
    public int levelMin;
    public int levelMax;
    public List<ChaseScatterTimings> chaseScatterTimings;
}

[System.Serializable]
public class ChaseScatterTimings
{
    public int scatterTiming;
    public int chaseTiming;
}
