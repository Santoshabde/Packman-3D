using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}
