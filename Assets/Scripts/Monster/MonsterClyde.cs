using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterClyde : Monster
{
    [SerializeField] private GameObject targetDebugPosition;

    private Vector3 TARGET_OFF_RADIUS = new Vector3(67,3,80); 
    protected override Vector3 CalculateTarget(PackmanLocomotion packman)
    {
        Vector3 target = TARGET_OFF_RADIUS;
        if(Mathf.Pow((transform.position.x - packman.transform.position.x),2) 
            + Mathf.Pow((transform.position.y - packman.transform.position.y), 2) +
            Mathf.Pow((transform.position.z - packman.transform.position.z), 2) < (Mathf.Pow(GridBuilder.NODE_DIMENSION * 8,2)))
        {
            target = packman.transform.position;
        }

        if(targetDebugPosition != null)
        targetDebugPosition.transform.position = target;

        return target;
    }

    private void OnDrawGizmos()
    {
        Handles.color = new Color(1,0,0, 0.2f);
        Handles.DrawSolidDisc(packman.transform.position, Vector3.up, GridBuilder.NODE_DIMENSION * 8);
    }
}
