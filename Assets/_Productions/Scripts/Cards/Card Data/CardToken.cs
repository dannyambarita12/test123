
[System.Serializable]
public struct CardToken
{
    public CardTokenType Type;
    public int MinValue;
    public int MaxValue;
}

public enum CardTokenType
{
    Physical = 0,
    Magical = 1,
    Mental = 2,
    Psychic = 3,
    Block = 4,
    Dodge = 5,
}