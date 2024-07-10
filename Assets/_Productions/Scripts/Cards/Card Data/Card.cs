using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Card
{
    public List<CardToken> Sequence => CardData.DiceDatas;

    [ShowInInspector]
    public string Id { get; private set; }
    [ShowInInspector]
    public CardData CardData { get; private set; }
    [ShowInInspector]
    public bool IsUsed { get; private set; }

    public Card(CardData cardData)
    {
        Id = RandomStringGenerator.GenerateRandomString(6);
        CardData = cardData;
        IsUsed = false;
    }

    public void UseCard(bool condition)
    {
        IsUsed = condition;
    }
}