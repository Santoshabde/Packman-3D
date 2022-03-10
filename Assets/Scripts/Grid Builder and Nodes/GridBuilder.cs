using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : SerializeSingleton<GridBuilder>
{
    public const float NODE_DIMENSION = 2.6948f;

    [SerializeField] private Node node;
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private float gridSpacing;
    //[SerializeField] private List<Vector2Int> blockedNodes;
    [SerializeField] private Vector2Int enemySafeHouse;

    public Vector2Int EnemySafeHouse => enemySafeHouse;

    public int Height => height;
    public int Width => width;

    private Node[,] gridNodes;
    public Node[,] GridNodes { get { return gridNodes; }}

    void Awake()
    {
        //BuildGrid(width, height);

        gridNodes = new Node[width, height];

        //Filling up the grid array!!
        for (int i = 0; i < transform.childCount; i++)
        {
            Node node = transform.GetChild(i).GetComponent<Node>();
            gridNodes[node.RowNumber, node.ColumnNumber] = node;

            //Only Testing!!
            if(node.NodeType == NodeType.Block)
            {
                node.transform.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        GridNodesNeighborsBuilder();
    }

    public void BuildGrid(int height, int width)
    {
        gridNodes = new Node[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node spawnedNode = Instantiate(node, NODE_DIMENSION * new Vector3(i, 0, j), Quaternion.identity);
                spawnedNode.transform.parent = this.transform;
                spawnedNode.transform.localScale -= gridSpacing * Vector3.one;
                gridNodes[i, j] = spawnedNode;
                spawnedNode.InitialiseNode(i, j, NodeType.Open);
            }
        }

       // SetBlockNodes();
       // GridNodesNeighborsBuilder();
    }

    private void GridNodesNeighborsBuilder()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                List<Node> neighbourNodes = new List<Node>();
                List<NeighbourNode> neighbourNodesWithDir = new List<NeighbourNode>();

                Node node = gridNodes[i, j];

                //4 Possible Neighbours!!
                if ((i + 1) < width && gridNodes[i + 1, j].NodeType == NodeType.Open)
                {
                    neighbourNodes.Add(gridNodes[i + 1, j]);
                    neighbourNodesWithDir.Add(new NeighbourNode() { node = gridNodes[i + 1, j], direction = Direction.left });
                }

                if ((i - 1) >= 0 && gridNodes[i - 1, j].NodeType == NodeType.Open)
                {
                    neighbourNodes.Add(gridNodes[i - 1, j]);
                    neighbourNodesWithDir.Add(new NeighbourNode() { node = gridNodes[i - 1, j], direction = Direction.right });
                }

                if ((j + 1) < height && gridNodes[i, j + 1].NodeType == NodeType.Open)
                {
                    neighbourNodes.Add(gridNodes[i, j + 1]);
                    neighbourNodesWithDir.Add(new NeighbourNode() { node = gridNodes[i, j + 1], direction = Direction.down });
                }

                if ((j - 1) >= 0 && gridNodes[i, j - 1].NodeType == NodeType.Open)
                {
                    neighbourNodes.Add(gridNodes[i, j - 1]);
                    neighbourNodesWithDir.Add(new NeighbourNode() { node = gridNodes[i, j - 1], direction = Direction.up });
                }

                node.AddNeighbourNodes(neighbourNodes);
                node.AddNeighbourNodesWithDirections(neighbourNodesWithDir);
            }
        }
    }
}
