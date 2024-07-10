using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

/// <summary>
/// Test class, nothing useful here
/// </summary>
public class ShrinkAndExpand : MonoBehaviour
{
    public Vector2 targetSize;
    public float speed = 7;

    private Vector2 startSize;
    private RectTransform selfRect;

    private void Awake() {
        selfRect = GetComponent<RectTransform>();
        startSize = selfRect.sizeDelta;
    }

    public void Expand(TweenCallback onComplete = null)
    {
        DOVirtual.Vector2(startSize, targetSize, speed, (v) =>
        {
            selfRect.sizeDelta = v;
        }).OnComplete(onComplete);
    }

    public void Shrink(TweenCallback onComplete = null)
    {
        var currentSize = selfRect.sizeDelta;
        DOVirtual.Vector2(currentSize, startSize, speed, (v) =>
        {
            selfRect.sizeDelta = v;
        }).OnComplete(onComplete);
    }

    /*private void Update() {
        if (isExpanded) {
            selfRect.sizeDelta = Vector2.Lerp(selfRect.sizeDelta, targetSize, speed * Time.deltaTime);
        } else {
            selfRect.sizeDelta = Vector2.Lerp(selfRect.sizeDelta, startSize, speed * Time.deltaTime);
        }
    }*/
}
