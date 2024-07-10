using DependencyInjection;
using Lean.Pool;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DiceModel
{
    public Unit Unit;
    public UnitDice SelectedDice;
    public DeckModel Deck;
}

public enum CardControlState
{
    None,
    HoverDice,
}

public class PlayerPhaseManager : MonoBehaviour
{   
    [Header("Card View")]
    [SerializeField]
    private CardView cardView;

    [ShowInInspector, ReadOnly]
    private DiceModel _activeModel;
    [ShowInInspector, ReadOnly]
    private DiceModel _targetModel;

    // State Machine
    [Header("Input State")]
    [SerializeField]
    private CardControlState _currentState;
    
    [Inject]
    private CombatManager _combatManager;
    
    [Inject]
    private GameStateManager _gameStateManager;
    
    private GameInput _gameInput;
    private Card _selectedCard;
    private DiceModel _hoveredModel;
    private DeckModel _currentDeck;
    private bool _isPlayerTurn;
    private UnitDiceTrajectoryDrawer _trajectoryDrawer;

    private void Start()
    {
        _gameStateManager[GameState.PlayerSetup].onEnter += OnEnterPlayerPhase;
        _gameStateManager[GameState.PlayerSetup].onExit += OnExitPlayerPhase;
        cardView.OnSelectCard.AddListener(OnSelectCard);
        cardView.SubsToPageEvent(OnOpenView, OnClosedView);
        _trajectoryDrawer = TrajectorySpawner.Spawn();

        DiceSelectEvent.Subscribe(OnSelectDice);
        DiceHoverEvent.Subscribe(OnHoverDice);
    }    

    private void OnDestroy()
    {
        DiceSelectEvent.Unsubscribe(OnSelectDice);
        DiceHoverEvent.Unsubscribe(OnHoverDice);
    }

    public void CreateCombatData(DiceModel source, DiceModel target, Card selectedCard)
    {
        var visualTrajectory = TrajectorySpawner.Spawn(source.SelectedDice.transform, target.SelectedDice.transform);

        var newCombatData = new CombatData
        {
            SourceUnit = source.Unit,
            TargetUnit = target.Unit,
            SourceDiceId = source.SelectedDice.DiceId,
            TargetDiceId = target.SelectedDice.DiceId,
            DiceValue = source.SelectedDice.DiceValue,
            UsedCard = selectedCard,
            TrajectoryDrawer = visualTrajectory,
        };

        _combatManager.RequestCombat(newCombatData);
    }

    [Inject]
    private void InjectGameInput(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.OnDeselectActionPerformed.AddListener(OnDeselectInputPressed);
    }

    private void OnEnterPlayerPhase(GameState state)
    {
        // Enable Input
        _activeModel = default;
        _targetModel = default;
        _hoveredModel = default;
        _selectedCard = null;
        _currentDeck = null;
        _currentState = CardControlState.None;
        _isPlayerTurn = true;
    }

    private void OnExitPlayerPhase(GameState state)
    {
        _isPlayerTurn = false;
        cardView.CloseCardView();
    }    

    private void OnDeselectInputPressed()
    {
        if(_isPlayerTurn == false)
            return;

        if (_selectedCard != null)
        {
            DeselectCard();
            return;
        }

        switch (_currentState)
        {
            case CardControlState.None:
                cardView.CloseCardView();
                break;
            
            case CardControlState.HoverDice:
                if (_hoveredModel.SelectedDice != null && _combatManager.HasCombatData(_hoveredModel.SelectedDice.DiceId, out var combatData))
                {
                    _hoveredModel.Deck.ReturnCard(combatData.UsedCard);
                    LeanPool.Despawn(combatData.TrajectoryDrawer);
                    _combatManager.RemoveCombat(combatData);
                }
                break;
        }
    }

    private void OnSelectDice(DiceModel model, bool isPlayer)
    {
        if (_isPlayerTurn == false)
            return;

        if (_activeModel.SelectedDice != null)
        {
            if (_activeModel.SelectedDice == model.SelectedDice)
                return;

            // Kalau lagi pegang kartu dan dice yang di klik itu dice musuh
            if (_selectedCard != null && model.Unit.IsPlayer == false)
            {
                SetAsTarget(model);
                return;
            }
        }

        if (isPlayer == false || _selectedCard != null)
            return;

        _activeModel = model;

        if (_currentDeck != null)
            _currentDeck.OnCardHandModified -= RefreshView;

        _currentDeck = _activeModel.Deck;
        _currentDeck.OnCardHandModified += RefreshView;

        cardView.OpenCardView();
        RefreshView(_currentDeck.HandCards);
    }

    private void RefreshView(List<Card> list)
    {
        cardView.RefreshCardView(list);
    }

    private void OnSelectCard(Card card)
    {
        _trajectoryDrawer.SetOrigin(_activeModel.SelectedDice.transform);
        _trajectoryDrawer.ShowTrajectory(true);

        _selectedCard = card;
    }
    
    private void DeselectCard()
    {
        _trajectoryDrawer.ClearTrajectory();
        _trajectoryDrawer.ShowTrajectory(false);
        cardView.ClearSelectedCard();

        _selectedCard = null;
    }

    private void OnHoverDice(DiceModel dice, bool isHovered)
    {
        _hoveredModel = dice;

        if (isHovered)
            ChangeState(CardControlState.HoverDice);
        else
            ChangeState(CardControlState.None);
    }

    private void OnOpenView()
    {
        
    }

    private void OnClosedView()
    {
        _activeModel = default;
        _targetModel = default;
        _selectedCard = null;

        _trajectoryDrawer.ClearTrajectory();
        _trajectoryDrawer.ShowTrajectory(false);
        ChangeState(CardControlState.None);
    }

    private void SetAsTarget(DiceModel model)
    {
        if (model.SelectedDice == _activeModel.SelectedDice || model.Unit == _activeModel.Unit)
        {
            Debug.Log("Can't target own unit");
            return;        
        }

        if(_selectedCard == null)
        {
            Debug.Log("Select card first");
            return;
        }
        
        if(_combatManager.HasCombatData(_activeModel.SelectedDice.DiceId, out var diceCombatData))
        {
            LeanPool.Despawn(diceCombatData.TrajectoryDrawer);
            _combatManager.RemoveCombat(diceCombatData);
        }

        _targetModel = model;

        CreateCombatData(_activeModel, _targetModel, _selectedCard);

        _activeModel.Deck.UseCard(_selectedCard);

        cardView.CloseCardView();
    }    

    private void ChangeState(CardControlState state)
    {
        _currentState = state;
    }
}
