using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Data", fileName = "New Card Data")]
public class CardData : BaseData
{
    [Title("Settings")]
    public int EnergyCost;
    public BaseStatType StatType;
    public CardActionType ActionType;
    public CardDistanceType DistanceType;

    [Title("Sequences")]
    [ListDrawerSettings(ShowFoldout = false)]
    public List<CardToken> DiceDatas = new();

    [Title("Data")]
    public CardSheet.Reference dataReference;

    [Button]
    public void RefreshData()
    {
        var data = dataReference.Ref;
        GenerateId(data.Id);
        Name = data.Name;
        EnergyCost = data.EP;
        StatType = data.StatType;
        ActionType = data.Action;
        DistanceType = data.Distance;

        DiceDatas.Clear();
        for(int i = 0; i < data.TokenCount; i++)
        {
            var cardToken = new CardToken()
            {
                Type = data.GetToken(i).TokenType,
                MinValue = data.GetToken(i).Min,
                MaxValue = data.GetToken(i).Max,
            };

            DiceDatas.Add(cardToken);
        }

        RenameFileData();
    }
}