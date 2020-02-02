using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{
    public Transform targetCamera;

    Vector3 movement = Vector3.zero;

    public float moveSpeed;

    public float smoothTurn;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        if (horInput != 0 || verInput != 0)
        {
            movement.x = horInput;
            movement.z = verInput;

            movement = movement.normalized;

            transform.Translate(movement * moveSpeed * Time.deltaTime,Space.Self);
            //rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
            //GetComponent<CharacterController>().Move(rb.position + movement * moveSpeed * Time.deltaTime);

            Quaternion tmp = targetCamera.rotation;
            targetCamera.eulerAngles = new Vector3(0f, targetCamera.eulerAngles.y, 0f);

            movement = targetCamera.TransformDirection(movement);
            targetCamera.rotation = tmp;

            Quaternion dir = Quaternion.LookRotation(movement);
            //transform.rotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, smoothTurn * Time.deltaTime);
        }
    }
}
