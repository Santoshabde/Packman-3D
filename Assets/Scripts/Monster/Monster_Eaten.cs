using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Eaten : IMonsterState
{
    private Vector2Int enemySafeHouse;
    private Node enemySafeHouseNode;
    private Node currentNode;
    private PathFindingAlgorithm pathFindingAlgo;
    private float monsterSpeed;

    //Pathfinding algo available!!
    NextPathNodeAlgorithm shortestNodeAlgorithm;

    public Monster_Eaten(Monster monster)
    {
        enemySafeHouse = GridBuilder.Instance.EnemySafeHouse;
        enemySafeHouseNode = GridBuilder.Instance.GridNodes[enemySafeHouse.x, enemySafeHouse.y];
        shortestNodeAlgorithm = new NextPathNodeAlgorithm();

        //You can use any here!!
        pathFindingAlgo = shortestNodeAlgorithm;
    }

    public void OnEnter(Monster monster)
    {
        currentNode = monster.CurrentNode;

        monster.transform.GetComponent<BoxCollider>().enabled = false;
        monster.BodyMesh.enabled = false;
        monster.MonsterParticleEffect.SetActive(false);

        monsterSpeed = monster.Speed + 13;
    }

    public void OnExecute(Monster monster)
    {
        //On reached safe area!!
        if ((enemySafeHouseNode.position - monster.transform.position).magnitude < 0.25f)
        {
            RandomPickBetweenChaseAndScatter(monster);
            return;
        }

        if ((currentNode.position - monster.transform.position).magnitude > 0.25f)
        {
            Vector3 nodeDirection = (currentNode.position - monster.transform.position).normalized;
            monster.transform.forward = nodeDirection;
            monster.transform.position += nodeDirection * Time.deltaTime * monsterSpeed;
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

            currentNode = pathFindingAlgo.CalculateNextNode(currentNode, enemySafeHouseNode.transform, neighbourNodesToConsider, true);
        }
    }

    public void OnExit(Monster monster)
    {
        monster.transform.GetComponent<BoxCollider>().enabled = true;
        monster.BodyMesh.enabled = true;
        monster.MonsterParticleEffect.SetActive(true);
    }

    private void RandomPickBetweenChaseAndScatter(Monster monster)
    {
        MonsterStateController monsterStateController = MonsterStateController.Instance;
        IMonsterState statePicked = monster.monsterChase;
        if (Random.Range(0, 2) == 0)
            statePicked = monster.monsterScatter;

        monsterStateController.ChangeMonsterState(monster, statePicked);
    }
}
