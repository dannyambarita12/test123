using UnityEngine;

[System.Serializable]
public class CombatData
{
    public Unit SourceUnit;
    public Unit TargetUnit;

    public string SourceDiceId;
    public string TargetDiceId;
    public int DiceValue;

    public Card UsedCard;
    public UnitDiceTrajectoryDrawer TrajectoryDrawer;
}