using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 offset;

    private void Awake()
    {
        offset = target.position - transform.position;    
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        transform.position = target.position - offset;
    }
}
