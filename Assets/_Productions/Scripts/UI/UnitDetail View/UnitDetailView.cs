using DependencyInjection;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class UnitDetailView : MonoBehaviour
{
    [Title("Unit Detail Container")]
    [SerializeField]
    private UnitDetailContainer leftDetailContainer;
    [SerializeField]
    private UnitDetailContainer rightDetailContainer;

    [Inject]
    private CombatManager _combatManager;
    [Inject]
    private GameInput _gameInput;
    [Inject]
    private GameStateManager _gameStateManager;

    private bool _isDisable;

    private void Start()
    {
        _isDisable = true;
        leftDetailContainer.ClosePeek();
        rightDetailContainer.ClosePeek();

        _gameInput.OnDeselectActionPerformed.AddListener(OnDeselectPressed);
        _gameStateManager[GameState.PlayerSetup].onEnter += OnEnterPlayerPhase;
        _gameStateManager[GameState.PlayerSetup].onExit += OnExitPlayerPhase;
    }

    private void OnEnterPlayerPhase(GameState state)
    {
        _isDisable = false;
    }

    private void OnExitPlayerPhase(GameState state)
    {
        _isDisable = true;
        leftDetailContainer.Select(false);
        rightDetailContainer.Select(false);
        leftDetailContainer.ClosePeek();
        rightDetailContainer.ClosePeek();
    }

    private void OnEnable()
    {
        UnitSelectedEvent.Subscribe(OnUnitSelected);
        UnitHoverEvent.Subscribe(OnUnitHover);

        DiceHoverEvent.Subscribe(OnDiceHover);
        DiceSelectEvent.Subscribe(OnDiceSelect);
    }

    private void OnDisable()
    {
        UnitSelectedEvent.Unsubscribe(OnUnitSelected);
        UnitHoverEvent.Unsubscribe(OnUnitHover);

        DiceHoverEvent.Unsubscribe(OnDiceHover);
        DiceSelectEvent.Unsubscribe(OnDiceSelect);
    }

    private void OnDeselectPressed()
    {
        if (_isDisable)
            return;

        leftDetailContainer.Select(false);
        leftDetailContainer.SelectCard(false);
        rightDetailContainer.Select(false);
        rightDetailContainer.SelectCard(false);
    }

    private void OnUnitHover(Unit unit, bool isHovered)
    {
        if (_isDisable)
            return;

        var container = GetContainer(unit.IsPlayer);
        
        if(isHovered)
            container.Peek(unit);
        else
            container.ClosePeek();
    }

    private void OnUnitSelected(bool isPlayer)
    {
        if(_isDisable)
            return;

        var container = GetContainer(isPlayer);
        container.Select(true);
    }

    private void OnDiceSelect(DiceModel model, bool isPlayer)
    {
        if(_isDisable)
            return;

        var container = GetContainer(isPlayer);
        container.Select(true);
        container.SelectCard(true);
    }

    private void OnDiceHover(DiceModel model, bool isHovered)
    {
        if (_isDisable)
            return;

        var container = GetContainer(model.Unit.IsPlayer);

        if (isHovered)
        {
            var card = GetDiceCard(model.SelectedDice.DiceId);
            container.Peek(model.Unit);

            if (card != null)
                container.PeekCard(card);
        }
        else
        {
            container.ClosePeek();
            container.ClosePeekCard();
        }
    }

    private UnitDetailContainer GetContainer(bool isPlayer)
    {
        return isPlayer ? leftDetailContainer : rightDetailContainer;
    }

    private Card GetDiceCard(string diceId)
    {
        if (_combatManager.HasCombatData(diceId, out var data) == false)
            return null;

        return data.UsedCard;
    }
}
