using CustomExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckModel
{
    public List<Card> HandCards;
    public List<Card> DeckCards;
    public List<Card> GraveCards;

    public Action<List<Card>> OnCardHandModified;

    public DeckModel(List<CardData> cardDatas)
    {
        HandCards = new List<Card>();
        GraveCards = new List<Card>();
        DeckCards = new List<Card>();

        for (int i = 0; i < cardDatas.Count; i++)
        {
            Card card = new Card(cardDatas[i]);
            DeckCards.Add(card);
        }
    }

    public void DrawCard(int cardAmount)
    {
        if (DeckCards.Count < cardAmount)
            ReturnGraveCardToDeck();

        HandCards.Clear();
        DeckCards.Shuffle();
        var reservedCards = new List<Card>();

        for (int i = 0; i < cardAmount; i++)
        {
            if (i >= DeckCards.Count)
            {
                Debug.LogWarning("Deck is empty or less than reserved amount");
                break;
            }

            var card = DeckCards[i];
            HandCards.Add(card);
            reservedCards.Add(card);
        }

        // To avoid error when removing card from list
        foreach (var reservedCard in reservedCards)
            DeckCards.Remove(reservedCard);
    }

    public void UseCard(Card card)
    {
        card.UseCard(true);
        OnCardHandModified?.Invoke(HandCards);
    }

    public void ReturnCard(Card card)
    {
        card.UseCard(false);
        OnCardHandModified?.Invoke(HandCards);
    }

    public void MoveHandCardToGrave()
    {
        GraveCards.AddRange(HandCards);
        HandCards.Clear();
    }

    private void ReturnGraveCardToDeck()
    {
        GraveCards.ForEach(card =>
        {
            card.UseCard(false);
            DeckCards.Add(card);
        });

        GraveCards.Clear();
    }
}
