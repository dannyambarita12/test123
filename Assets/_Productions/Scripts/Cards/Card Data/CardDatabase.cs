using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Database", fileName = "New Card Database")]
public class CardDatabase : BaseDatabase<CardData>
{
    [Button("REFRESH CARD DATA", ButtonSizes.Large)]
    public void RefreshCardData()
    {
        foreach (var card in Items)
        {
            card.RefreshData();
        }
    }
}
