using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class UnitDiceTrajectoryDrawer : MonoBehaviour
{
    [SerializeField]
    private int numberOfPoints = 100;
    [SerializeField]
    private AnimationCurve curve;

    private Transform _startPoint;
    private Transform _endPoint;

    private LineRenderer _lineRenderer;
    private bool _isSelectingCardTarget;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawTrajectoryToMousePointer();
    }

    public void SetOrigin(Transform origin)
    {
        _startPoint = origin;
    }

    public void SetTarget(Transform target)
    {
        _endPoint = target;
    }

    public void ShowTrajectory(bool condition)
    {
        _isSelectingCardTarget = condition;
    }

    public void ClearTrajectory()
    {
        _lineRenderer.positionCount = 0;
        _lineRenderer.SetPositions(new Vector3[0]);
    }

    public void DrawTrajectory()
    {
        Vector3[] positions = new Vector3[numberOfPoints + 1];

        for (int i = 0; i <= numberOfPoints; i++)
        {
            float t = i / (float)numberOfPoints;
            positions[i] = Vector3.Lerp(_startPoint.position, _endPoint.position, t);
            positions[i].y = positions[i].y + curve.Evaluate(t);
        }

        _lineRenderer.positionCount = numberOfPoints + 1;
        _lineRenderer.SetPositions(positions);
    }

    private void DrawTrajectoryToMousePointer()
    {
        if (!_isSelectingCardTarget)
            return;
        
        Vector3[] positions = new Vector3[numberOfPoints + 1];

        for (int i = 0; i <= numberOfPoints; i++)
        {
            float t = i / (float)numberOfPoints;
            positions[i] = Vector3.Lerp(_startPoint.position, GetMousePositionOrtho(), t);
            positions[i].y = positions[i].y + curve.Evaluate(t);
        }

        _lineRenderer.positionCount = numberOfPoints + 1;
        _lineRenderer.SetPositions(positions);
    }   

    private Vector3 GetMousePositionOrtho()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        return mousePosition;
    }
}
