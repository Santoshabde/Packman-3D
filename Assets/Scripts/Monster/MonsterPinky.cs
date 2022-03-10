using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterPinky : Monster
{
    [SerializeField] private GameObject targetDebugPosition;
    protected override Vector3 CalculateTarget(PackmanLocomotion packman)
    {
        Vector3 offset = Vector3.zero;
        if (packman.CurrentDirection == Direction.up)
            offset = new Vector3(10.7792f, 0, 0);

        Vector3 targetPosition = (packman.transform.position + (packman.transform.forward * 10.7792f)) + new Vector3(0, 2, 0) + offset;

        if(targetDebugPosition != null)
        targetDebugPosition.transform.position = targetPosition;

        return targetPosition;
    }
}
