using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    none,
    up,
    down,
    left,
    right
}

public class PackmanInput : MonoBehaviour
{
    public static Direction currentDirection = Direction.none;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentDirection = Direction.left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = Direction.right;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentDirection = Direction.up;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = Direction.down;
        }
    }
}
