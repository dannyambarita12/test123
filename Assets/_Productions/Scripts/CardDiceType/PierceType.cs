using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pierce Type", fileName = "PierceType")]
public class PierceType : CardDiceType
{

    public override void ProcessClashWinOutcome(Unit attackerUnit, Unit defenderUnit, CardDiceType defenderCardDiceType, int attackerDiceValue, int defenderDiceValue)
    {
        defenderCardDiceType.ProcessClashLoseOutcomeByAttackType(attackerUnit, defenderUnit, attackerDiceValue, defenderDiceValue);
    }

    public override void ProcessClashLoseOutcomeByAttackType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal full damage
        //defenderUnit.TakeDamage(attackerDiceValue);
    }

    public override void ProcessClashLoseOutcomeByGuardType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker deal substracted stagger damage

        int totalDamage = attackerDiceValue - defenderDiceValue;
        //defenderUnit.TakeStaggerDamage(totalDamage);
    }

    public override void ProcessClashLoseOutcomeByEvadeType(Unit attackerUnit, Unit defenderUnit, int attackerDiceValue, int defenderDiceValue)
    {
        // attacker gain stagger resist
        // attacker retain evade dice

        //attackerUnit.RecoverStaggerResist(attackerDiceValue);
    }

    public override void ProcessOneSidedOutcome(Unit defenderUnit, int diceValue)
    {
        // target unit take damage

        //defenderUnit.TakeDamage(diceValue);
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
