using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster_Frigtened : IMonsterState
{
    private Node currentNode;
    private Color originalColor;

    public void OnEnter(Monster monster)
    {
        Debug.Log("Frighting Enter");

        originalColor = monster.BodyMesh.GetComponent<Renderer>().material.color;
        monster.BodyMesh.GetComponent<Renderer>().material.color = Color.blue;

        //When frightened, enemies make 180 degree turn first, and then pick up the random nearest tile!!
        monster.transform.forward = Quaternion.AngleAxis(180, Vector3.up) * monster.transform.forward;

        currentNode = monster.CurrentNode;
    }

    public void OnExecute(Monster monster)
    {
        //Pick up the random neighbour tile, except the tile behind monster(Monster don't go back)
        if ((currentNode.transform.position - monster.transform.position).magnitude > 0.1f)
        {
            Vector3 nodeDirection = (currentNode.position - monster.transform.position).normalized;
            monster.transform.forward = nodeDirection;
            monster.transform.position += nodeDirection * Time.deltaTime * monster.Speed;
        }
        else
        {
            //Snap it may be - Not sure!!
            //monster.transform.position = currentNode.position;

            List<Node> neighbourNodes = new List<Node>();
            neighbourNodes.AddRange(currentNode.NeighbourNodes);

            //Removing node behind the monster!!
            foreach (var node in neighbourNodes)
            {
                if (Vector3.Dot(monster.transform.forward, (node.position - monster.transform.position).normalized) < -0.3f)
                {
                    neighbourNodes.Remove(node);
                    break;
                }
            }

            int randomNeighbour = Random.Range(0, neighbourNodes.Count);
            currentNode = neighbourNodes[randomNeighbour];
        }
    }

    public void OnExit(Monster monster)
    {
        Debug.Log("Frighting exit");
        monster.BodyMesh.GetComponent<Renderer>().material.color = originalColor;
    }
}
