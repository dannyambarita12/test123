using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleCardDiceClashHandler : Singleton<SingleCardDiceClashHandler>
{
    public IEnumerator StartSingleCardDiceClash(CardToken attackerCardDice, Unit attackerUnit, CardToken defenderCardDice, Unit defenderUnit)
    {
        /*if (attackerCardDice.Type != DiceData.DiceType.NotSet && defenderCardDice.Type != DiceData.DiceType.NotSet)
        {
            // Card dice clash

            yield return StartCoroutine(MoveTargetUnitForClash(attackerUnit, defenderUnit));

            int attackerUnitDiceValue = RollCardDice(attackerCardDice);
            int defenderUnitDiceValue = RollCardDice(defenderCardDice);

            if (attackerUnitDiceValue > defenderUnitDiceValue)
            {
                // Attacker win

                attackerCardDice.cardDiceType.ProcessClashWinOutcome(attackerUnit, defenderUnit, defenderCardDice.cardDiceType, attackerUnitDiceValue, defenderUnitDiceValue);
            }
            else if (attackerUnitDiceValue < defenderUnitDiceValue)
            {
                // Defender win

                defenderCardDice.cardDiceType.ProcessClashWinOutcome(attackerUnit, defenderUnit, attackerCardDice.cardDiceType, attackerUnitDiceValue, defenderUnitDiceValue);
            }
            else
            {
                Debug.Log("clash is draw");
            }

            yield return new WaitForSeconds(.5f);
        }
        else if (attackerCardDice.Type != DiceData.DiceType.NotSet && defenderCardDice.Type == DiceData.DiceType.NotSet)
        {
            // One sided card dice attack from A to B

            yield return StartCoroutine(MoveTargetUnitForClash(defenderUnit, attackerUnit));

            int diceValue = RollCardDice(attackerCardDice);
            attackerCardDice.cardDiceType.ProcessOneSidedOutcome(defenderUnit, diceValue);
        }
        else if (attackerCardDice.Type == DiceData.DiceType.NotSet && defenderCardDice.Type == DiceData.DiceType.NotSet)
        {
            // One sided card dice attack from B to A

            yield return StartCoroutine(MoveTargetUnitForClash(attackerUnit, defenderUnit));

            int diceValue = RollCardDice(defenderCardDice);
            defenderCardDice.cardDiceType.ProcessOneSidedOutcome(attackerUnit, diceValue);
        }
        else
        {
            // Both card dice has nothing
        }*/

        yield return null;
    }

    private IEnumerator MoveTargetUnitForClash(Unit attackerUnit, Unit defenderUnit)
    {
        if (defenderUnit.transform.position.x > attackerUnit.transform.position.x)
        {
            Vector3 movePosition = attackerUnit.transform.position + Vector3.right;
            //CameraMovement.Instance.MoveCameraToBetweenUnitClash(attackerUnit.transform.position, movePosition);
            yield return defenderUnit.transform.DOMove(movePosition, .5f).SetEase(Ease.Linear).WaitForCompletion();
        }
        else
        {
            Vector3 movePosition = attackerUnit.transform.position + Vector3.left;
            //CameraMovement.Instance.MoveCameraToBetweenUnitClash(attackerUnit.transform.position, movePosition);
            yield return defenderUnit.transform.DOMove(movePosition, .5f).SetEase(Ease.Linear).WaitForCompletion();
        }
    }

    private int RollCardDice(CardToken cardDice)
    {
        return Random.Range(cardDice.MinValue, cardDice.MaxValue + 1);
    }
}
