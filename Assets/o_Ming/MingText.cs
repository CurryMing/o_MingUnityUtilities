using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using o_Ming;

public class MingText : MonoBehaviour
{
    public float moveSpeed;

    public GameObject m_camera;
    public float followSpeed;

    private Rigidbody2D rb;

    public float force;

    public GameObject line;
    private LineRenderer lr;
    public float lr_Length;
    private Vector2 lr_point2;

    Vector2 forceDir = Vector2.zero;
    RaycastHit2D hit;

    Vector2 maxSpeed = Vector2.zero;

    public LayerMask layerMask;
    public LayerMask wallLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = line.GetComponent<LineRenderer>();
        Debug.Log(Application.persistentDataPath);
    }

    private void Update()
    {
        Ming.CameraFollow(transform, m_camera.transform, followSpeed);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (rb.velocity.y >= 15 || rb.velocity.y <= -15)
        {
            float Vx = Mathf.Clamp(rb.velocity.x, -15f, 15f);
            float Vy = Mathf.Clamp(rb.velocity.y, -15f, 15f);
            rb.velocity = new Vector2(Vx, Vy);
        }

        if (Input.GetMouseButton(0))
        {
            line.transform.position = transform.position;
            lr.enabled = true;
            Time.timeScale = 0.2f;

            Ming.TurnToTarget2D(line.transform, mousePos, true);
            DrawReflection2D(line.transform.position, line.transform.up);
        }
        if (Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
            Time.timeScale = 1f;
            lr_point2 = lr.GetPosition(1);
            maxSpeed = rb.velocity;
            forceDir = (lr_point2 - (Vector2)line.transform.position).normalized;
            rb.velocity = line.transform.up * force;
        }

    }

    private void DrawReflection2D(Vector2 originPosition, Vector2 direction)
    {
        

        originPosition += direction * 0.7f;

        lr.SetPosition(0, originPosition);

        hit = Physics2D.Raycast(originPosition, direction, lr_Length);
        if (hit.collider != null)
        {
            lr.SetPosition(1, hit.point);

            float remainLength = lr_Length - (hit.point - (Vector2)lr.GetPosition(0)).magnitude;

            direction = Vector2.Reflect(direction, hit.normal);

            lr.SetPosition(2, (hit.point + direction.normalized * remainLength));
        }
        else
        {
            lr.SetPosition(1, (originPosition + direction.normalized * lr_Length));
            lr.SetPosition(2, (originPosition + direction.normalized * lr_Length));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("block"))
        {
            
            Vector2 wallDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = wallDir.normalized * moveSpeed;
        }
    }
}
