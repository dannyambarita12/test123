using CustomExtensions;
using DependencyInjection;
using System;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField]
    private SceneCountUI sceneCountUI;
    [SerializeField]
    private RollAndCombatButtonUI rollAndCombatButton;

    [Inject]
    private GameStateManager _gameStateManager;

    private int _roundCount;

    private void Start()
    {
        _gameStateManager[GameState.RoundStart].onEnter += OnRoundStart;
        rollAndCombatButton.SubscribeToButton(GoToRollPhase, GoToCombatPhase);

        sceneCountUI.Hide();
        rollAndCombatButton.SetActive(false);
    }

    private void OnRoundStart(GameState state)
    {
        rollAndCombatButton.SetActive(false);
        sceneCountUI.ShowRoundCountBanner(++_roundCount, ShowRollAndCombatButton);
    }

    private void ShowRollAndCombatButton()
    {
        rollAndCombatButton.SetActive(true);
        rollAndCombatButton.ShowRollButton(true);
    }

    private void GoToRollPhase()
    {
        _gameStateManager.ChangeState(GameState.RollDice);
    }

    private void GoToCombatPhase()
    {
        _gameStateManager.ChangeState(GameState.Combat);
    }
}
