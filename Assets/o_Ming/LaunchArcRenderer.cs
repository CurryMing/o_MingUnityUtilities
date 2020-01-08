using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer : MonoBehaviour
{
    public Transform player;

    private JoyStick joyStick;

    private LineRenderer lr;
    public float velocity;
    public float angle;
    public int resolution;

    private float g;  //force of gravity on the y axis
    private float radianAngle;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
        joyStick = FindObjectOfType<JoyStick>();
    }


    private void Start()
    {

        RenderArc();
    }

    private void Update()
    {
        if (lr!=null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    void RenderArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }
    //create an array of Vector3 positions for arc
    public Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(radianAngle * 2)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalcalateArcPoint(t,maxDistance);        
        }

        return arcArray;
    }
    //calculate height and distance of each vertex
    Vector3 CalcalateArcPoint(float t,float distance)
    {
        float x = t * distance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
