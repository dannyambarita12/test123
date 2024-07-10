using DependencyInjection;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour, IDependencyProvider
{
    public bool AutoCombat { get; private set; }

    public UnityEvent OnSelectActionPerformed;
    public UnityEvent OnDeselectActionPerformed;
    public UnityEvent OnSpacePerformed;
    public UnityEvent OnAutoTargetPerformed;

    private PlayerInputActions _playerInputActions;

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.RightMouse.performed += Deselect_performed;
        _playerInputActions.Player.LeftMouse.performed += Select_performed;
        _playerInputActions.Player.AutoTarget.performed += AutoTarget_Performed;
        _playerInputActions.Player.Space.performed += Space_Performed;
    }

    [Button]
    public void ToggleAutoCombat(bool condition)
    {
        AutoCombat = condition;
    }

    private void Space_Performed(InputAction.CallbackContext context)
    {
        OnSpacePerformed?.Invoke();
    }

    private void AutoTarget_Performed(InputAction.CallbackContext context)
    {
        OnAutoTargetPerformed?.Invoke();
    }

    private void Select_performed(InputAction.CallbackContext context)
    {
        OnSelectActionPerformed?.Invoke();
    }

    private void Deselect_performed(InputAction.CallbackContext context)
    {
        OnDeselectActionPerformed?.Invoke();
    }

    [Provide]
    private GameInput ProvideGameInput()
    {
        return this;
    }
}
