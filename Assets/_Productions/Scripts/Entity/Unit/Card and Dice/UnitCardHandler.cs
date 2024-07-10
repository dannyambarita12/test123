using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCardHandler : MonoBehaviour
{
    public bool IsPlayer => _unit.IsPlayer;
    public List<UnitDice> ActiveDices => _activeDices;

    [Title("Deck"), PropertyOrder(-1)]
    [SerializeField]
    private DeckModel deck;

    [Title("Initial Cards")]
    [SerializeField]
    private CardData[] unitCards;

    [SerializeField]
    private UnitDice[] unitDices;

    public int diceAmount = 1;

    private List<UnitDice> _activeDices = new();

    private Unit _unit;

    public void Initialize(Unit ownerUnit)
    {
        _unit = ownerUnit;        
        SetupCard();

        foreach (var unitDice in unitDices)
        {            
            unitDice.OnPressedDice.AddListener(SelectDice);
            unitDice.OnEnterDice.AddListener(HoverDice);
            unitDice.OnExitDice.AddListener(ExitDice);
            unitDice.ActivateDice(false);
        }
    }

    public void PrepareUnit()
    {
        // TODO:: Make calculation for setup dice amount
        SetupDice(diceAmount);
        deck.DrawCard(2);
    }

    public void RollDice()
    {
        foreach (UnitDice unitDice in unitDices)
        {
            if (unitDice.IsActive == false)
                continue;

            unitDice.SetDiceValue(Random.Range(2, 8));
        }        
    }

    public Card GetRandomCard()
    {
        if (deck.HandCards.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, deck.HandCards.Count);
            return deck.HandCards[randomIndex];
        }

        return null;
    }

    public UnitDice GetRandomDice()
    {
        return _activeDices[UnityEngine.Random.Range(0, _activeDices.Count)];
    }

    public void ResetDeck()
    {
        deck.MoveHandCardToGrave();        
    }

    public void HideDice()
    {
        foreach (var dice in _activeDices)
        {
            dice.ActivateDice(false);
            dice.SetDiceValueToTentative();
        }
    }

    private void HoverDice(UnitDice dice)
    {
        var diceModel = GenerateModel(dice);

        if (IsPlayer)
            DiceHoverEvent.Invoke(diceModel, true);
        else
            DiceEnemyHoverEvent.Invoke(diceModel, true);
    }

    private void ExitDice(UnitDice dice)
    {
        var diceModel = GenerateModel(dice);

        if (IsPlayer)
            DiceHoverEvent.Invoke(diceModel, false);
        else
            DiceEnemyHoverEvent.Invoke(diceModel, false);
    }    
    
    private void SelectDice(UnitDice dice)
    {
        var cardModel = GenerateModel(dice);
        DiceSelectEvent.Invoke(cardModel, IsPlayer);
    }

    private void SetupCard()
    {
        deck = new DeckModel(unitCards.ToList());
    }

    private void SetupDice(int amount)
    {
        _activeDices.Clear();

        for (int i = 0; i < unitDices.Length; i++)
        {
            var dice = unitDices[i];

            bool isActive = i < amount;
            dice.ActivateDice(isActive);

            if (isActive)
                _activeDices.Add(unitDices[i]);
        }
    }

    private DiceModel GenerateModel(UnitDice dice)
    {
        return new DiceModel
        {
            Unit = _unit,
            SelectedDice = dice,
            Deck = deck,
        };
    }
}