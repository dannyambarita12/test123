using DependencyInjection;
using System;
using UnityEngine;

public class DiceTargetingSystem : MonoBehaviour
{
    private UnitDice _currentHighlightedDice;
    private UnitDice selectedDice;
    private UnitDice targetDice;
    private CardUI highlightedCard;
    private CardUI selectedCard;

    private void GameInput_OnSelectActionPerformed()
    {
        if (highlightedCard != null)
        {
            selectedCard = highlightedCard;
            
            return;
        }

        if (_currentHighlightedDice != null)
        {
            SetTargetingDice(_currentHighlightedDice);
            return;
        }
    }

    private void GameInput_OnDeselectActionPerformed()
    {
        
    }

    private void SetTargetingDice(UnitDice unitDice)
    {
        
    }

    private void EvaluateCardTargeting()
    {
        
    }

    public void SetHighlightedDice(UnitDice unitDice)
    {
        _currentHighlightedDice = unitDice;
    }

    public void ClearHighlightedDice()
    {
        _currentHighlightedDice = null;
    }

    public UnitDice GetHighlightedDice()
    {
        return _currentHighlightedDice;
    }

    public void SetSelectedDice(UnitDice unitDice)
    {
        selectedDice = unitDice;
    }

    public bool HasSelectedDice()
    {
        return selectedDice != null;
    }

    public void SetHighlightedCard(CardUI card)
    {
        highlightedCard = card;
    }

    public void ClearHighlightedCard()
    {
        highlightedCard = null;
    }

    public void SetSelectedCard(CardUI card)
    {
        selectedCard = card;
    }

    public UnitDice GetSelectedDice()
    {
        return selectedDice;
    }
}
