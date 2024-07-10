using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTrajectoryTest : MonoBehaviour
{
    
    public Transform startPoint;
    public Transform endPoint;
    public int numberOfPoints = 100;
    public AnimationCurve animationCurve;

    private LineRenderer lineRenderer;

    
    private void Update()
    {
        DrawTrajectory();

    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void DrawTrajectory()
    {
        Vector3[] positions = new Vector3[numberOfPoints + 1];

        for (int i = 0; i <= numberOfPoints; i++)
        {
            float t = i / (float)numberOfPoints;
            positions[i] = Vector3.Lerp(startPoint.position, endPoint.position, t);
            positions[i].y = animationCurve.Evaluate(t);
        }

        lineRenderer.positionCount = numberOfPoints + 1;
        lineRenderer.SetPositions(positions);
    }
}
