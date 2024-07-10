using Cathei.BakingSheet.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitData : BaseData
{
    [TitleGroup("Settings")]
    [HorizontalGroup("Settings/Properties", Width = 100)]
    [PreviewField(100, Alignment = ObjectFieldAlignment.Left)]
    [HideLabel]
    public Sprite Portrait;

    [HorizontalGroup("Settings/Properties")]
    [VerticalGroup("Settings/Properties/Stat"), OnValueChanged(nameof(SimulateStat))]
    [Range(1, 20)]
    public int Level;
    [VerticalGroup("Settings/Properties/Stat"), OnValueChanged(nameof(SimulateStat))]
    [Range(1, 20)]
    public int StrValue;
    [VerticalGroup("Settings/Properties/Stat"), OnValueChanged(nameof(SimulateStat))]
    [Range(1, 20)]
    public int DexValue;
    [VerticalGroup("Settings/Properties/Stat"), OnValueChanged(nameof(SimulateStat))]
    [Range(1, 20)]
    public int IntValue;

    [TitleGroup("Card Data")]
    public CardDatabase cards;

    [TitleGroup("Settings")]
    [ShowInInspector, ReadOnly, HideLabel]
    private DynamicStat dynamicStat;

    [TitleGroup("Debug"), PropertyOrder(100)]
    [ButtonGroup("Debug/Randomize")]
    private void RandomData()
    {
        StrValue = Random.Range(1, 10);
        DexValue = Random.Range(1, 10);
        IntValue = Random.Range(1, 10);
        
        SimulateStat();
    }

    private void SimulateStat()
    {
        dynamicStat = StatCalculator.GetSimulatedStat(Level, StrValue, DexValue, IntValue);
    }    
}

[System.Serializable]
public struct DynamicStat
{
    public int Health;
    public int Mental;
    public int Speed;
    public int ActionPoint;
    public int StaminaPoint;
}