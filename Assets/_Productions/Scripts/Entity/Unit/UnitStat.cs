using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UnitStat : MonoBehaviour
{
    [Title("DYNAMIC STATS")]
    [ShowInInspector]
    public int Health { get; private set; }
    [ShowInInspector]
    public int Mental { get; private set; }
    [ShowInInspector]
    public int SpeedPoint { get; private set; }
    [ShowInInspector]
    public int ActionPoint { get; private set; }
    [ShowInInspector]
    public int StaminaPoint { get; private set; }

    [Title("BASE STATS")]
    [ShowInInspector]
    public int Str { get; private set; }
    [ShowInInspector]
    public int Dex { get; private set; }
    [ShowInInspector]
    public int Int { get; private set; }

    [PropertyOrder(5)]
    public UnityEvent<UnitStat> onValueChange = new();

    public void InitStats(int level, int strength, int dexterity, int intelligence)
    {
        Str = strength;
        Dex = dexterity;
        Int = intelligence;

        var dynamicStat = StatCalculator.GetSimulatedStat(level, strength, dexterity, intelligence);
        Health = dynamicStat.Health;
        Mental = dynamicStat.Mental;
        SpeedPoint = dynamicStat.Speed;
        ActionPoint = dynamicStat.ActionPoint;
        StaminaPoint = dynamicStat.StaminaPoint;

        onValueChange?.Invoke(this);
    }
}
