using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotation : MonoBehaviour
{
    private float rotationSpeed = 200f;
    private void Update()
    {
        transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
    }
}
