using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TokenImageDatabase", menuName = "Utilities/TokenImageDatabase", order = 1)]
public class TokenImageDatabase : SerializedScriptableObject
{
    [DictionaryDrawerSettings]
    public Dictionary<CardDistanceType, Sprite> cardDistanceIconDictionary = new();

    [DictionaryDrawerSettings()]
    public Dictionary<CardTokenType, TokenAssetData> tokenImageDictionary = new();

    public TokenAssetData GetTokenImage(CardTokenType tokenType)
    {
        if (tokenImageDictionary.ContainsKey(tokenType))
        {
            return tokenImageDictionary[tokenType];
        }
        else
        {
            Debug.LogError("TokenImageDatabase: TokenImage not found for tokenType: " + tokenType);
            return null;
        }
    }

    public Sprite GetCardDistanceIcon(CardDistanceType cardDistanceType)
    {
        if (cardDistanceIconDictionary.ContainsKey(cardDistanceType))
        {
            return cardDistanceIconDictionary[cardDistanceType];
        }
        else
        {
            Debug.LogError("TokenImageDatabase: CardDistanceIcon not found for cardDistanceType: " + cardDistanceType);
            return null;
        }
    }
}

[Serializable]
public class TokenAssetData
{
    public Sprite tokenSprite;
    public Color tokenColor;
}
