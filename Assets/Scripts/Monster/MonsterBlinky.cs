using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBlinky : Monster
{
    [SerializeField] private Transform targetDebugPosition;
    protected override Vector3 CalculateTarget(PackmanLocomotion packman)
    {
        Vector3 targetPosition = base.CalculateTarget(packman);

        if(targetDebugPosition != null)
        targetDebugPosition.position = targetPosition;

        return targetPosition;
    }
}
