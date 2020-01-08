using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform target;

    public float h = 25;
    public float gravity = -18;

    public bool debugPath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
        if (debugPath)
        {
            DrawPath();
        }
    }

    public void Launch()
    {
        Physics2D.gravity = Vector2.up * gravity;
        rb.velocity = CalculateLaunchData().initialVelocity;
        //Debug.Log(rb.velocity);
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - rb.position.y;

        Vector2 displacementXY = new Vector2(target.position.x - rb.position.x, displacementY);

        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);

        Vector2 velocityXY = displacementXY / time;

        return new LaunchData((velocityXY + velocityY) /* -Mathf.Sign(gravity)*/,time);
    }

    struct LaunchData
    {
        public readonly Vector2 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector2 velocity, float time)
        {
            this.initialVelocity = velocity;
            this.timeToTarget = time;
        }
    }

    void DrawPath()
    {
        Vector2 previousDrawPoint = rb.position;

        LaunchData launchData = CalculateLaunchData();

        float resolution = 30f;
        for (int i = 0; i < resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;

            Vector2 displacement = launchData.initialVelocity * simulationTime + Vector2.up * gravity * simulationTime * simulationTime / 2f;

            Vector2 drawPoint = rb.position + displacement;

            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

            previousDrawPoint = drawPoint;
        }

    }
}
