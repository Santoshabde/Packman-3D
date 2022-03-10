using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PathFindingAlgorithm
{
    Node CalculateNextNode(Node startNode, Transform endNode, Node[,] nodesGrid, bool isTowards);

    Node CalculateNextNode(Node startNode, Transform endNode, List<Node> startNodeNeighbours, bool isTowards);
    Node CalculateNextNode(Node startNode, Vector3 endNode, List<Node> startNodeNeighbours, bool isTowards);
}
