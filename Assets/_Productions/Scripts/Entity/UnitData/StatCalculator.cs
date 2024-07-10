using UnityEngine;

public static class  StatCalculator
{
    public static Vector2Int BaseSpeed = new Vector2Int(1, 6);
    public static int BaseActionPoint = 1;
    public static int BaseStaminaPoint = 3;

    public static DynamicStat GetSimulatedStat(int level, int strValue, int dexValue, int intValue)
    {
        DynamicStat dynamicStat = new DynamicStat();
        dynamicStat.Health = GetHealth(level, strValue);
        dynamicStat.Mental = GetMental(level, intValue);
        dynamicStat.Speed = GetSpeed(level, dexValue);
        dynamicStat.ActionPoint = GetActionPoint(level, dexValue);
        dynamicStat.StaminaPoint = GetStaminaPoint(level, strValue, dexValue);

        return dynamicStat;
    }

    public static int GetHealth(int level, int strength)
    {
        return (5 * (level + 1)) + Mathf.CeilToInt(level * strength / 2);
    }

    public static int GetMental(int level, int intelligence)
    {
        return (2 * (level + 1)) + Mathf.CeilToInt(level * intelligence / 2);
    }

    public static int GetSpeed(int level, int dexterity)
    {
        var speed = Random.Range(BaseSpeed.x, BaseSpeed.y);
        return speed + GetLevelSpeed(level) + GetDexSpeed(dexterity);
    }

    public static int GetActionPoint(int level, int dexterity)
    {
        return BaseActionPoint + GetLevelAction(level) + GetDexAction(dexterity);
    }

    public static int GetStaminaPoint(int level, int strength, int dexterity)
    {
        return BaseStaminaPoint + GetStrengthStamina(strength) + GetDexterityStamina(dexterity);
    }

    public static int GetLevelSpeed(int level)
    {
        return Mathf.FloorToInt((float)level / 5f);
    }

    public static int GetDexSpeed(int dex)
    {
        return Mathf.FloorToInt((float)dex / 5f);
    }

    public static int GetLevelAction(int level)
    {
        return Mathf.FloorToInt((float)level / 10f);
    }

    public static int GetDexAction(int dex)
    {
        return Mathf.FloorToInt((float)dex / 10f);
    }

    public static int GetStrengthStamina(int strength)
    {
        return Mathf.FloorToInt((float)strength / 5f);
    }

    public static int GetDexterityStamina(int dex)
    {
        return Mathf.FloorToInt((float)dex / 5f);
    }
}