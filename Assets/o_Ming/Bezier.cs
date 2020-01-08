 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bezier : MonoBehaviour
{
    private JoyStick joyStick;

    BallLauncher ballLauncher;

    public LineRenderer lr;
    public Transform point0, point1, point2,point3;

    private int numOfPoint = 50;

    Vector3[] positions;

    public float lineLength = 3;
    public float lineHeight = 3;

    private void Awake()
    {
        lr.positionCount = numOfPoint;
        positions = new Vector3[numOfPoint];
        joyStick = FindObjectOfType<JoyStick>();
        ballLauncher = FindObjectOfType<BallLauncher>();
        
    }

    private void Update()
    {
        DrawLinearCurve();
    }

    private void DrawLinearCurve()
    {
        for (int i = 1; i < numOfPoint + 1; i++)
        {
            float t = i / (float)numOfPoint;
            //positions[i - 1] = CalculateLinearBezierPoint(t, point0.position, point1.position);
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.position, point1.position,point2.position);
            //positions[i - 1] = CalculateCubicBezierPoint(t, point0.position, point1.position, point2.position,point3.position);
        }
        lr.SetPositions(positions);
    }

    private Vector3 CalculateLinearBezierPoint(float t,Vector3 p0,Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (t * t - 2 * t + 1) * p0 + (-2 * t * t + 2 * t) * p1 + t * t * p2;
    }
    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2,Vector3 p3)
    {
        //
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        Vector3 Pt = uuu * p0 + 3 * t * uu * p1 + 3 * tt * u * p2 + tt * t * p3;
        return Pt;
    }
}
