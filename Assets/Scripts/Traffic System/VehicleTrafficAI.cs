using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VehicleTrafficAI : MonoBehaviour
{
    private const float DESTINATION_LIMIT = 0.5f;

    [SerializeField] private List<WayPoints> wayPoints;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private int wayPointNumber;
    private void Awake()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(ReachedDestination(wayPoints[wayPointNumber].wayPoint.position))
        {
            wayPointNumber += 1;

            if(wayPointNumber >= wayPoints.Count)
            {
                wayPointNumber = 0;
            }
        }

        navMeshAgent.SetDestination(wayPoints[wayPointNumber].wayPoint.position);
    }

    private bool ReachedDestination(Vector3 destination)
    {
        if((destination - transform.position).magnitude < DESTINATION_LIMIT)
        {
            return true;
        }

        return false;
    }

    [System.Serializable]
    public class WayPoints
    {
        public Transform wayPoint;
        public float waitTime;
    }

    private void OnDrawGizmos/*Selected*/()
    {
        Gizmos.color = Color.blue;
        foreach (var item in wayPoints)
        {
            Gizmos.DrawSphere(item.wayPoint.position, 1f);
            item.wayPoint.position = Handles.PositionHandle(item.wayPoint.position, item.wayPoint.rotation);
        }

        for (int i = 0; i < wayPoints.Count; i++)
        {
            if((i + 1) < wayPoints.Count)
            Gizmos.DrawLine(wayPoints[i].wayPoint.position, wayPoints[i + 1].wayPoint.position);
        }
    }
}
