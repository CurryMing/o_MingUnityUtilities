using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    private Rigidbody rb;

    private float inputHorizontalValue = 0f;
    //private float inputVerticalValue = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputHorizontalValue = Input.GetAxisRaw("Horizontal");
        //inputVerticalValue = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 yRot = new Vector3(0f, inputHorizontalValue, 0f);
        Quaternion rotation = Quaternion.Euler(yRot);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
