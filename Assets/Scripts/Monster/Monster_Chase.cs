using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monster_Chase : IMonsterState
{
    private Node currentNode;
    private PathFindingAlgorithm pathFindingAlgo;

    //Pathfinding algo available!!
    NextPathNodeAlgorithm shortestNodeAlgorithm;

    public Monster_Chase()
    {
        shortestNodeAlgorithm = new NextPathNodeAlgorithm();

        //You can use any here!!
        pathFindingAlgo = shortestNodeAlgorithm;
    }

    public void OnEnter(Monster monster)
    {
        Debug.Log("Chase enter");

        //Activate Body
        monster.BodyMesh.enabled = true;

        currentNode = monster.CurrentNode;
    }

    public void OnExecute(Monster monster)
    {
        //On Target Reached!! - Currently just stopping monster on reaching the target!!
        if ((monster.Target - monster.transform.position).magnitude < 0.1f)
            return;

        if((currentNode.position - monster.transform.position).magnitude > 0.1f)
        {
            Vector3 nodeDirection = (currentNode.position - monster.transform.position).normalized;
            monster.transform.forward = nodeDirection;
            monster.transform.position += nodeDirection * Time.deltaTime * monster.Speed;
        }
        else
        {
            List<Node> neighbourNodesToConsider = new List<Node>();
            neighbourNodesToConsider.AddRange(currentNode.NeighbourNodes); //Simply cant equate, because list is reference type!!

            //Removing node behind the monster!!
            foreach (var node in neighbourNodesToConsider)
            {
                if (Vector3.Dot(monster.transform.forward, (node.position - monster.transform.position).normalized) < -0.3f)
                {
                    neighbourNodesToConsider.Remove(node);
                    break;
                }
            }
     
            currentNode = pathFindingAlgo.CalculateNextNode(currentNode, monster.Target, neighbourNodesToConsider, true);
        }
    }

    public void OnExit(Monster monster)
    {
        Debug.Log("Chase exit");
    }
}
