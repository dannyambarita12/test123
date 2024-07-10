using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("Card Container and References")]
    [SerializeField]
    private Transform cardContainer;
    [SerializeField]
    private CardUI cardPrefab;

    [Header("Page Components")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;

    public UnityEvent<Card> OnSelectCard = new();
    public UnityEvent onOpen;
    public UnityEvent onClose;

    private List<CardUI> _cardSlotItems = new();
    private CardUI _highlightedCardUI;
    private bool _isOpen;

    public void SubsToPageEvent(UnityAction onOpenCallback, UnityAction onCloseCallback)
    {
        if(onOpenCallback != null)
            onOpen.AddListener(onOpenCallback);

        if(onCloseCallback != null)
            onClose.AddListener(onCloseCallback);
    }

    public void RefreshCardView(List<Card> cards)
    {
        DestroyCard();
        _cardSlotItems.Clear();

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].IsUsed)
                continue;

            CardUI card = Instantiate(cardPrefab, cardContainer);
            card.SetCard(cards[i], SelectCard);
            _cardSlotItems.Add(card);
        }
    }

    public void OpenCardView()
    {
        if(_isOpen)
            return;

        _isOpen = true;
        ActiveView(_isOpen);
        onOpen?.Invoke();
    }

    public void CloseCardView()
    {
        if(_isOpen == false)
            return;

        _isOpen = false;
        ActiveView(_isOpen);
        onClose?.Invoke();
    }

    public void ClearSelectedCard()
    {
        DisableCardUIPeek(false);
        _highlightedCardUI.HighlightCard(false);
        _highlightedCardUI = null;
    }

    private void SelectCard(CardUI cardSlot)
    {
        _highlightedCardUI = cardSlot;
        cardSlot.HighlightCard(true);
        DisableCardUIPeek(true);
        OnSelectCard?.Invoke(cardSlot.Card);
    }    

    private void DisableCardUIPeek(bool condition)
    {
        foreach (CardUI card in _cardSlotItems)
        {
            if(card == _highlightedCardUI)
                continue;

            card.DisablePeek(condition);
        }
    }

    private void DestroyCard()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
    }    

    private void ActiveView(bool condition)
    {
        canvas.enabled = condition;
        graphicRaycaster.enabled = condition;    
    }
}
