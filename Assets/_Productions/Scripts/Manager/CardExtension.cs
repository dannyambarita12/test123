public static class CardExtension
{
    public static bool IsAttack(this CardTokenType tokenType)
    {
        return tokenType == CardTokenType.Physical || tokenType == CardTokenType.Mental || tokenType == CardTokenType.Magical;
    }

    public static bool IsParry(CardDistanceType performerDistanceType, CardDistanceType targetDistanceType, CardTokenType targetToken)
    {
        return targetToken.IsAttack() && (performerDistanceType == CardDistanceType.Melee && targetDistanceType == CardDistanceType.Ranged);
    }
}