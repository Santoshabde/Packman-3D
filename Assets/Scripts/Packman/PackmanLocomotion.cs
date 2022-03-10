using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PackmanLocomotion : MonoBehaviour
{
    #region Constants
    private const string EAT_ANIMATION_BOOL_PARAMETER = "eating";
    #endregion

    #region Inspector exposed Variables
    //Inspector exposed!!
    [SerializeField] private float speed;
    [SerializeField] private Vector2Int currentPackmanPosition;
    [SerializeField] private Animator animator;
    #endregion

    #region Private variables
    private Direction currentDirection;
    private Direction previousDirection;

    private bool disableInput;
    #endregion

    public static Action<bool> OnWallHit;

    public Direction CurrentDirection => currentDirection;

    void Start()
    {
        PackmanCollisionsController.OnPackmanDeath += OnPackmanDeath;
        PackmanVFXManager.OnPackManRespawn += OnPackmanRespawn;
        ScoreAndLevelManager.OnGameWin += OnPackmanGameWin;

        currentDirection = Direction.left;
        PackmanInput.currentDirection = Direction.left;
    }

    void Update()
    {
        if (!EnablePackmanInput())
            return;

        if (currentDirection == Direction.right)
        {
            MovePackman(Quaternion.Euler(0, -90, 0), Direction.right);
        }

        if (currentDirection == Direction.left)
        {
            MovePackman(Quaternion.Euler(0, 90, 0), Direction.left);
        }

        if (currentDirection == Direction.up)
        {
            MovePackman(Quaternion.Euler(0, -180, 0), Direction.up);
        }

        if (currentDirection == Direction.down)
        {
            MovePackman(Quaternion.Euler(0, 0, 0), Direction.down);
        }
    }

    //Rotate packman to rotationToRotate and move packman in specified direction!!
    private void MovePackman(Quaternion rotationToRotate, Direction directionToMove)
    {
        Node selectedNode = null;
        Vector2Int selectedNodePosition = Vector2Int.zero;
        foreach (var item in GridBuilder.Instance.GridNodes[currentPackmanPosition.x, currentPackmanPosition.y].NeighbourNodesWithDirection)
        {
            if (item.direction == directionToMove)
            {
                selectedNode = item.node;
                selectedNodePosition = new Vector2Int(item.node.RowNumber, item.node.ColumnNumber);
            }
        }
        if (selectedNode != null)
        {
            OnWallHit?.Invoke(false);
            animator.SetBool(EAT_ANIMATION_BOOL_PARAMETER, true);
            previousDirection = currentDirection;
            currentDirection = PackmanInput.currentDirection;
            transform.rotation = rotationToRotate;
            transform.position = Vector3.MoveTowards(transform.position, selectedNode.position, Time.deltaTime * speed);

            if (transform.position == selectedNode.position)
            {
                currentPackmanPosition = selectedNodePosition;
            }
        }
        else
        {
            if (currentDirection == previousDirection)
            {
                OnWallHit?.Invoke(true);
                animator.SetBool(EAT_ANIMATION_BOOL_PARAMETER, false);
                currentDirection = PackmanInput.currentDirection;
            }

            else
            {
                currentDirection = previousDirection;
            }
        }
    }


    #region On Game Lost and Win Locomotion Changes
    private bool EnablePackmanInput() => !disableInput && LevelBuilder.startGame && LevelBuilder.buildLevel;

    private void OnPackmanDeath()
    {
        disableInput = true;
        animator.SetBool(EAT_ANIMATION_BOOL_PARAMETER, false);
        transform.GetComponent<SphereCollider>().enabled = false;
    }

    private void OnPackmanRespawn()
    {
        currentPackmanPosition = new Vector2Int(8, 19);
        Invoke("EnableCollider", 3f);
        transform.position = GridBuilder.Instance.GridNodes[currentPackmanPosition.x, currentPackmanPosition.y].position;
        disableInput = false;
        currentDirection = Direction.left;
        PackmanInput.currentDirection = Direction.left;

    }
    void EnableCollider()
    {
        transform.GetComponent<SphereCollider>().enabled = true;
    }

    private void OnPackmanGameWin()
    {
        disableInput = true;
        animator.SetBool(EAT_ANIMATION_BOOL_PARAMETER, false);
        transform.GetComponent<SphereCollider>().enabled = false;
    }
    #endregion
}
