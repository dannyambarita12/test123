using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guard Type", fileName = "GuardType")]
public abstract class CardDiceType : ScriptableObject
{
    public abstract void ProcessClashWinOutcome(Unit attackerUnit, Unit defenderUnit, CardDiceType defenderCardDiceType, int attackerDiceValue, int defenderDiceValue);
    public abstract void ProcessClashLoseOutcomeByAttackType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue);
    public abstract void ProcessClashLoseOutcomeByGuardType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue);
    public abstract void ProcessClashLoseOutcomeByEvadeType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue);
    public abstract void ProcessOneSidedOutcome(Unit defenderUnit, int diceValue);

    public abstract void ProcessClashWin(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue);
    public abstract void ProcessClashLose(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue);
}
