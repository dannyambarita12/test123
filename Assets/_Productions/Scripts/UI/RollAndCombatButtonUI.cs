using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollAndCombatButtonUI : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    [SerializeField] private Button initiateCombatButton;

    private Action _onRollButtonClicked;
    private Action _onCombatButtonClicked;

    private void Start()
    {
        rollButton.onClick.AddListener(() =>
        {
            ShowRollButton(false);
            _onRollButtonClicked?.Invoke();

        });

        initiateCombatButton.onClick.AddListener(() =>
        {
            ShowInitiateCombatButton(false);
            _onCombatButtonClicked?.Invoke();
        });
    }

    public void SubscribeToButton(Action onRollButtonClicked, Action onCombatButtonClicked)
    {
        _onRollButtonClicked = onRollButtonClicked;
        _onCombatButtonClicked = onCombatButtonClicked;
    }

    public void ShowRollButton(bool condition)
    {
        rollButton.gameObject.SetActive(condition);
        ShowInitiateCombatButton(!condition);
    }

    public void ShowInitiateCombatButton(bool condition)
    {
        initiateCombatButton.gameObject.SetActive(condition);
    }
}
