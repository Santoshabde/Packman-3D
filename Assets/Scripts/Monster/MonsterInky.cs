using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInky : Monster
{
    [SerializeField] private GameObject targetDebugPosition;
    [SerializeField] private MonsterBlinky monsterBlinky;
    protected override Vector3 CalculateTarget(PackmanLocomotion packman)
    {
        Vector3 offset = Vector3.zero;
        if (packman.CurrentDirection == Direction.up)
            offset = new Vector3(10.7792f, 0, 0);

        Vector3 targetPosition = (packman.transform.position + (packman.transform.forward * 10.7792f)) + new Vector3(0, 2, 0) + offset;

        Vector3 targetPositionToRedMonster = (monsterBlinky.transform.position + new Vector3(0, 2, 0)) - targetPosition;
        Debug.DrawRay(targetPosition, targetPositionToRedMonster, Color.red);

        Vector3 rotatedVector = Quaternion.AngleAxis(180, Vector3.up) * targetPositionToRedMonster;
        Debug.DrawRay(targetPosition, rotatedVector, Color.blue);

        if(targetDebugPosition != null)
        targetDebugPosition.transform.position = rotatedVector + targetPosition;

        return rotatedVector + targetPosition;
    }
}
