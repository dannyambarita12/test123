using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera defaultVirtualCamera;
    [SerializeField] 
    private CinemachineVirtualCamera combatVirtualCamera;
    [SerializeField]
    private CinemachineTargetGroup targetGroupCamera;
    [SerializeField] 
    private Vector3 defaultPosition;
    [SerializeField] 
    private Vector3 sceneStartPosition;
    [SerializeField] 
    private float zValueOnZoom;
    [SerializeField] 
    private float moveDuration;

    private void Start()
    {
        transform.position = sceneStartPosition;
        transform.DOMove(defaultPosition, 1.5f);
    }

    public void ActivateCombatCamera(bool condition)
    {
        combatVirtualCamera.Priority = condition ? 11 : -1;
    }

    public void SetTarget(Transform targetA, Transform targetB)
    {
        targetGroupCamera.m_Targets[0].target = targetA;
        targetGroupCamera.m_Targets[1].target = targetB;
    }

    public IEnumerator MoveCameraToDefaultPosition()
    {
        // yield return new WaitForSeconds(.25f);
        transform.DOMove(defaultPosition, 1.5f);
        yield break;
    }

    public void MoveCameraToBetweenUnitClash(Vector3 positionA, Vector3 positionB)
    {
        var targetPosition = Vector3.Lerp(positionA, positionB, .5f);
        targetPosition.z = zValueOnZoom;
        transform.DOMove(targetPosition, moveDuration);
    }
    
    public void MoveCameraTo(Vector3 targetPosition)
    {
        targetPosition.z = zValueOnZoom;
        transform.DOMove(targetPosition, moveDuration);
    }
}
