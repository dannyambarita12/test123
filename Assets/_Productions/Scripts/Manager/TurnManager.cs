using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public enum State { PreDiceRoll, PostDiceRoll, Combat, PostCombat }

public class TurnManager : Singleton<TurnManager>
{

    public Action OnPreDiceRollPhaseStart;
    public Action OnPostDiceRollPhaseStart;
    public Action OnCombatPhaseStart;
    public Action OnPostCombatPhaseStart;
    public Action OnShowSceneCount;
    public Action OnHideSceneCount;
    public Action OnBattleEnd;

    [Header("State")]
    public State state;

    [SerializeField] private float startingDelay = 0.5f;
    [SerializeField] private float postCombatPhaseDuration = 2f;

    private void Start()
    {
        StartCoroutine(StartPreDiceRollPhaseAfterDelay(startingDelay));
    }

    public void SetStateToCombat()
    {
        SetBattleState(State.Combat);
    }

    public void SetStateToPreDiceRoll()
    {
        SetBattleState(State.PreDiceRoll);
    }

    public void SetStateToPostDiceRoll()
    {
        SetBattleState(State.PostDiceRoll);
    }

    public void SetStateToPostCombat()
    {
        SetBattleState(State.PostCombat);
    }

    private IEnumerator StartPreDiceRollPhaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetBattleState(State.PreDiceRoll);
    }

    private void SetBattleState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.PreDiceRoll:
                StartPreDiceRollPhase();
                break;
            case State.PostDiceRoll:
                StartPostDiceRollPhase();
                break;
            case State.Combat:
                StartCombatPhase();
                break;
            case State.PostCombat:
                StartPostCombatPhase();
                break;
        }
    }

    private void StartPreDiceRollPhase()
    {
        OnPreDiceRollPhaseStart?.Invoke();
    }

    private void StartPostDiceRollPhase()
    {
        OnPostDiceRollPhaseStart?.Invoke();
    }

    private void StartCombatPhase()
    {   
        OnCombatPhaseStart?.Invoke();
    }

    private void StartPostCombatPhase()
    {
        OnPostCombatPhaseStart?.Invoke();

        StartCoroutine(StartPreDiceRollPhaseAfterDelay(postCombatPhaseDuration));
    }
}
