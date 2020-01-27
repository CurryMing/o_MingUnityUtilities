using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed;
    private float rotY;
    private Vector3 offset;


    private void Awake()
    {

        rotY = transform.eulerAngles.y;
        offset = target.position - transform.position;

    }

    private void LateUpdate()
    {
        rotY += Input.GetAxis("Mouse X") * rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);

    }
}
