using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Evade Type", fileName = "EvadeType")]
public class EvadeType : CardDiceType
{

    public override void ProcessClashWinOutcome(Unit attackerUnit, Unit defenderUnit, CardDiceType defenderCardDiceType, int attackerDiceValue, int defenderDiceValue)
    {
        defenderCardDiceType.ProcessClashLoseOutcomeByEvadeType(attackerUnit, defenderUnit, attackerDiceValue, defenderDiceValue);
    }

    public override void ProcessClashLoseOutcomeByAttackType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal full damage

        //defenderUnit.TakeDamage(attackerDiceValue);
    }

    public override void ProcessClashLoseOutcomeByGuardType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal full stagger damage

        //defenderUnit.TakeStaggerDamage(attackerDiceValue);
    }

    public override void ProcessClashLoseOutcomeByEvadeType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // both dice are negated
    }

    public override void ProcessOneSidedOutcome(Unit defenderUnit, int diceValue)
    {
        // retain dice
    }

    public override void ProcessClashWin(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue)
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessClashLose(Unit ownerUnit, CardDiceType opponentDiceType, int ownerDiceValue, int opponentDiceValue)
    {
        throw new System.NotImplementedException();
    }
}
