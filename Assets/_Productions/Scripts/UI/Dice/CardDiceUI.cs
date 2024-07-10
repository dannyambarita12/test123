using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDiceUI : MonoBehaviour
{
    [SerializeField]
    private CardUI cardUI;

    public void ShowTooltip()
    {

    }

    public void HideTooltip()
    {

    }

    public void SetCard(Card card)
    {
        cardUI.SetCard(card, null);
    }

    public void ExpandCard(bool condition)
    {
        cardUI.HighlightCard(condition);
    }
}
