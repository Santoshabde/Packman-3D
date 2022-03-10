using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextPathNodeAlgorithm : PathFindingAlgorithm
{
    public Node CalculateNextNode(Node startNode, Transform endNode, Node[,] nodesGrid, bool isTowardsTarget)
    {
        List<float> distances = new List<float>();
        List<Node> neighborNodes = new List<Node>();

        //Pick up the next current Node!!
        foreach (var item in startNode.NeighbourNodes)
        {
            distances.Add((endNode.position - item.position).sqrMagnitude);
            neighborNodes.Add(item);
        }

        float distance = isTowardsTarget? distances.Min() : distances.Max();
        int distanceIndex = distances.IndexOf(distance);

        return neighborNodes[distanceIndex];
    }

    public Node CalculateNextNode(Node startNode, Transform endNode, List<Node> startNodeNeighboursToConsider, bool isTowardsTarget)
    {
        List<float> distances = new List<float>();
        List<Node> neighborNodes = new List<Node>();

        foreach (var item in startNodeNeighboursToConsider)
        {
            distances.Add((endNode.position - item.position).sqrMagnitude);
            neighborNodes.Add(item);
        }

        float distance = isTowardsTarget ? distances.Min() : distances.Max();
        int distanceIndex = distances.IndexOf(distance);

        return neighborNodes[distanceIndex];
    }

    public Node CalculateNextNode(Node startNode, Vector3 endNode, List<Node> startNodeNeighboursToConsider, bool isTowardsTarget)
    {
        List<float> distances = new List<float>();
        List<Node> neighborNodes = new List<Node>();

        foreach (var item in startNodeNeighboursToConsider)
        {
            distances.Add((endNode - item.position).sqrMagnitude);
            neighborNodes.Add(item);
        }

        float distance = isTowardsTarget ? distances.Min() : distances.Max();
        int distanceIndex = distances.IndexOf(distance);

        return neighborNodes[distanceIndex];
    }
}
