using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Open = 0,
    Block = 1
}

[System.Serializable]
public class NeighbourNode
{
    [ReadOnly] public Node node;
    [ReadOnly] public Direction direction;
}

public class Node : MonoBehaviour
{
    [SerializeField] private NodeType nodeType;
    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField, ReadOnly] private List<Node> neighbourNodes;
    [SerializeField] private List<NeighbourNode> neighbourNodesWithDirections;

    #region Properties
    public int RowNumber { get { return rowNumber; } }
    public int ColumnNumber { get { return columnNumber; } }
    public NodeType NodeType { get { return nodeType; } set { nodeType = value; } }

    public List<Node> NeighbourNodes
    {
        get
        {
            return neighbourNodes;
        }
    }

    public List<NeighbourNode> NeighbourNodesWithDirection
    {
        get
        {
            return neighbourNodesWithDirections;
        }
    }
    #endregion

    public Vector3 position;

    public void InitialiseNode(int rowNumber, int columnNumber, NodeType nodeType)
    {
        this.rowNumber = rowNumber;
        this.columnNumber = columnNumber;
        this.nodeType = nodeType;
        position = this.transform.position;
    }

    public void AddNeighbourNodes(List<Node> neighbours)
    {
        neighbourNodes = neighbours;
    }

    public void AddNeighbourNodesWithDirections(List<NeighbourNode> neighbours)
    {
        neighbourNodesWithDirections = neighbours;
    }
}
