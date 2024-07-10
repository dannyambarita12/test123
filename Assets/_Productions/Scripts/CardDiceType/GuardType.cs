using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guard Type", fileName = "GuardType")]
public class GuardType : CardDiceType
{

    public override void ProcessClashWinOutcome(Unit attackerUnit, Unit defenderUnit, CardDiceType defenderCardDiceType, int attackerDiceValue, int defenderDiceValue)
    {
        defenderCardDiceType.ProcessClashLoseOutcomeByGuardType(attackerUnit, defenderUnit, attackerDiceValue, defenderDiceValue);
    }

    public override void ProcessClashLoseOutcomeByAttackType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal substracted damage

        int totalDamage = attackerDiceValue - defenderDiceValue;
        //defenderUnit.TakeDamage(totalDamage);
    }

    public override void ProcessClashLoseOutcomeByGuardType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal full stagger damage

        //defenderUnit.TakeStaggerDamage(attackerDiceValue);
    }

    public override void ProcessClashLoseOutcomeByEvadeType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        //  attacker recover stagger resist

        //attackerUnit.RecoverStaggerResist(attackerDiceValue);
    }

    public override void ProcessOneSidedOutcome(Unit defenderUnit, int diceValue)
    {
        // retain dice
    }

    public override void ProcessClashWin(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue)
    {
        throw new NotImplementedException();
    }

    public override void ProcessClashLose(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue)
    {
        throw new NotImplementedException();
    }
}
