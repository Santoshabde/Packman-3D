using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject packManToFollow;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float lerpSpeed;
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, packManToFollow.transform.position + cameraOffset, Time.deltaTime * lerpSpeed);
    }
}
