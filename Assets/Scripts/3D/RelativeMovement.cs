using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{
    public Transform targetCamera;

    Vector3 movement = Vector3.zero;

    public float moveSpeed;

    public float smoothTurn;

    Vector3 mouseDir = Vector3.zero;

    private void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        if (horInput != 0 || verInput != 0)
        {
            movement.x = horInput;
            movement.z = verInput;

            transform.Translate(movement * moveSpeed * Time.deltaTime,Space.World);

            Quaternion tmp = targetCamera.rotation;
            targetCamera.eulerAngles = new Vector3(0f, targetCamera.eulerAngles.y, 0f);

            mouseDir = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            mouseDir = targetCamera.TransformDirection(mouseDir);

            

            targetCamera.rotation = tmp;

            Quaternion dir = Quaternion.LookRotation(mouseDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, smoothTurn * Time.deltaTime);
            
        }
    }
}
