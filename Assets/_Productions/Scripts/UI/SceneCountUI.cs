using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sceneCountText;

    private Action _onFinish;

    public void ShowRoundCountBanner(int roundCount, Action onFinisedCallback)
    {
        sceneCountText.text = $"Scene {roundCount}";
        _onFinish = onFinisedCallback;

        Show();
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        Hide();
        _onFinish?.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
