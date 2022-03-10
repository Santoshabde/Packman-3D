using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Scatter : IMonsterState
{
    private Node targetNode;
    private Node nextNode;
    private Node[,] gridNodes;
    private List<Vector2Int> idleRoamNodes;
    private int currentIdleRoamIndex;
    private PathFindingAlgorithm pathFindingAlgo;

    #region pathfinding algos available
    //Pathfinding algo available!!
    private NextPathNodeAlgorithm shortestNodeAlgorithm;
    #endregion

    public Monster_Scatter(Monster monster)
    {
       // gridNodes = GridBuilder.Instance.GridNodes;
        gridNodes = GridBuilder.Instance.GridNodes;
        idleRoamNodes = monster.IdleRoamNodes;
        shortestNodeAlgorithm = new NextPathNodeAlgorithm();

        //You can use any here!!
        pathFindingAlgo = shortestNodeAlgorithm;
    }

    public void OnEnter(Monster monster)
    {
        Debug.Log("Scatter Enter");

        //Activate Body
        monster.BodyMesh.enabled = true;

        //Calculate target node - which is first node of idle roam nodes!!
        currentIdleRoamIndex = 0;
        targetNode = gridNodes[idleRoamNodes[currentIdleRoamIndex].x, idleRoamNodes[currentIdleRoamIndex].y];
        //targetNode.GetComponent<Renderer>().material.color = Color.white;

        //calculate next node!!
        nextNode = pathFindingAlgo.CalculateNextNode(monster.CurrentNode, targetNode.transform, gridNodes, true);
    }

    public void OnExecute(Monster monster)
    {
        //On reached target, change target node to next node in Idle roam nodes(circularly)
        if ((targetNode.position - monster.transform.position).magnitude < 0.1f)
        {
            currentIdleRoamIndex += 1;
            if (currentIdleRoamIndex >= idleRoamNodes.Count)
                currentIdleRoamIndex = 0;

            targetNode = gridNodes[idleRoamNodes[currentIdleRoamIndex].x, idleRoamNodes[currentIdleRoamIndex].y];
           //targetNode.GetComponent<Renderer>().material.color = Color.white;

            nextNode = pathFindingAlgo.CalculateNextNode(nextNode, targetNode.transform, gridNodes, true);
            return;
        }

        //This code section, where monster tries to reach target node!!
        if ((nextNode.position - monster.transform.position).magnitude > 0.1f)
        {
            Vector3 nodeDirection = (nextNode.position - monster.transform.position).normalized;
            monster.transform.forward = nodeDirection;
            monster.transform.position += nodeDirection * Time.deltaTime * monster.Speed;
        }
        else
        {
            List<Node> neighbourNodesToConsider = new List<Node>();
            neighbourNodesToConsider.AddRange(nextNode.NeighbourNodes); //Simply cant equate, because list is reference type!!

            //Removing node behind the monster!!
            foreach (var node in neighbourNodesToConsider)
            {
                if (Vector3.Dot(monster.transform.forward, (node.position - monster.transform.position).normalized) < -0.3f)
               {
                    neighbourNodesToConsider.Remove(node);
                    break;
                }
            }

            nextNode = pathFindingAlgo.CalculateNextNode(nextNode, targetNode.transform, neighbourNodesToConsider, true);
        }
    }

    public void OnExit(Monster monster)
    {
        Debug.Log("Scatter exit");
    }
}
