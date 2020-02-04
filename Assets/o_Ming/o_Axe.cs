using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using o_Ming;

public class o_Axe : MonoBehaviour
{
    Vector2 targetPos = Vector2.zero;

    public Transform rayPos;
    public LayerMask blockMask;
    private Vector2 size = new Vector2(0, 0.38f);
    private float range = 0.38f;

    public float moveSpeed;
    public float backSpeed;
    public float rotateSpeed = 450f;

    public GameObject _camera;
    public float shakeTime;
    public float shakeSpeed;
    public float shakeAmount;

    private Transform player;

    private Rigidbody2D rb2D;

    private bool isToTarget = false;

    public Transform axeOriginPos;

    public ParticleSystem backToPlayerEffect;

    private bool effectIsPlaying = true;
    private bool isRotating = false;

    public TrailRenderer upTrail;
    //public TrailRenderer downTrail;

    private void Awake()
    {
        player = FindObjectOfType<o_Dash>().transform;
        rb2D = GetComponent<Rigidbody2D>();
        targetPos = transform.position;
    }
    private void Update()
    {
        AxeIsOnPlayer();

        AxeIsOnTarget();

        if (isRotating == true)
        {
            RotateAxe();
            upTrail.enabled = true;
            //downTrail.enabled = true;
        }

        if (isToTarget == false)
        {
            MoveToTarget();
        }
        else
        {
            BackToPlayer();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayPos.position, rayPos.position + rayPos.transform.right * range);
    }

    private void AxeIsOnPlayer()
    {
        if (Vector3.Distance(transform.position, axeOriginPos.position) < 0.1)
        {
            //TODO(BackToPlayerEffect)
            ReturnEffect();
            upTrail.enabled = false;
            //downTrail.enabled = false;

            if (Input.GetMouseButtonDown(0))
            {
                isRotating = true;
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isToTarget = false;
            }

        }
    }
    private void AxeIsOnTarget()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.01)
        {
            isRotating = false;

            if (Input.GetMouseButtonDown(0))
            {
                effectIsPlaying = false;

                isToTarget = true;
            }
        }
    }

    private void MoveToTarget()
    {
        rb2D.position = Vector2.MoveTowards(rb2D.position, targetPos, moveSpeed * Time.deltaTime);
    }
    private void BackToPlayer()
    {
        Ming.TurnToTarget2D(transform, player.position, true);
        //rb2D.rotation = 0f;
        rb2D.position = Vector2.MoveTowards(rb2D.position, axeOriginPos.position, backSpeed * Time.deltaTime);
    }
    private void ReturnEffect()
    {
        if (effectIsPlaying == false)
        {
            effectIsPlaying = true;
            StartCoroutine(Ming.CameraShake(_camera, shakeSpeed, shakeTime, shakeAmount));
            backToPlayerEffect.Play();
        }
    }
    private void RotateAxe()
    {
        transform.Rotate(0, 0, rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "item")
        {
            StartCoroutine(Ming.CameraShake(_camera, shakeSpeed, shakeTime, shakeAmount));
        }
    }
}
