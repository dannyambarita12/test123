using DependencyInjection;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VadimskyiLab.Events;

public class GameStateManager : MonoBehaviour, IDependencyProvider
{
    public StateMachine<GameState>.StateHooks this[GameState state] => _stateMachine[state];

    [ShowInInspector]
    public GameState CurrentState { get; private set; }
    [ShowInInspector]
    public GameState PreviousState { get; private set; }

    private StateMachine<GameState> _stateMachine = new StateMachine<GameState>();
    
    [Provide]
    private GameStateManager Provide() => this;

    private void Start()
    {
        GameStateChangeEvent.Subscribe(ChangeState);
    }

    private void OnDestroy()
    {
        _stateMachine.UnsubscribeAll();
    }

    private void Update()
    {
        _stateMachine.Update(CurrentState, PreviousState);
    }

    [Button("Debug Change State")]
    public void ChangeState(GameState state)
    {
        if (state == CurrentState) return;
        PreviousState = CurrentState;
        CurrentState = state;
    }

    [Button("Debug Next State")]
    public void NextState()
    {
        ChangeState(CurrentState + 1);
    }    
}

public enum GameState
{
    None = 0,
    Initialize = 1,
    RoundStart = 2,
    RollDice = 3,
    EnemySetup = 4,
    PlayerSetup = 5,
    Combat = 6,
    RoundEnd = 7,
    GameOver = 8,
}

public class GameStateChangeEvent : EventBase<GameStateChangeEvent, GameState> { }