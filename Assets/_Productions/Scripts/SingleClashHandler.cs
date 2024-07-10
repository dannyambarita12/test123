using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleClashHandler : MonoBehaviour
{
    [SerializeField] private SingleCardDiceClashHandler singleCardDiceClashHandler;

    private List<CardToken> _selectedUnitCardDiceDataList = new();
    private List<CardToken> _targetUnitCardDiceDataList = new();
    private CardData _selectedUnitCard;
    private CardData _targetUnitCard;
    private Unit _selectedUnit;
    private Unit _targetUnit;

    private Unit _attackerUnit;
    private Unit _defenderUnit;
    private int _attackerDiceValue;
    private int _defenderDiceValue;

    public IEnumerator StartSingleCombat(List<UnitDice> combatQueueList, UnitDice selectedDice)
    {
        /*UnitDice targetDice = selectedDice.GetTargetDice();

        if (IsTargetingEachOther(selectedDice, targetDice))
        {
            // Clash

            _selectedUnit = selectedDice.GetDiceOwnerUnit();
            _targetUnit = targetDice.GetDiceOwnerUnit();
            _selectedUnitCard = selectedDice.GetSelectedCard().GetCardDataSO();
            _targetUnitCard = targetDice.GetSelectedCard().GetCardDataSO();
            
            AddAllDiceDataToList(_selectedUnitCardDiceDataList, _selectedUnitCard);
            AddAllDiceDataToList(_targetUnitCardDiceDataList, _targetUnitCard);
            
            while (_selectedUnitCardDiceDataList.Count > 0 || _targetUnitCardDiceDataList.Count > 0)
            {
                yield return StartCoroutine(
                    StartSingleCardDiceClash(
                        _selectedUnitCardDiceDataList[0], 
                        _selectedUnit, 
                        _targetUnitCardDiceDataList[0],
                        _targetUnit));
            }
            
            combatQueueList.Remove(selectedDice);
            selectedDice.ClearTargetDice();
            selectedDice.ClearSelectedCard();

            combatQueueList.Remove(targetDice);
            targetDice.ClearTargetDice();
            targetDice.ClearSelectedCard();
        }
        else
        {
            // One-sided attack

            _selectedUnit = selectedDice.GetDiceOwnerUnit();
            _targetUnit = targetDice.GetDiceOwnerUnit();
            _selectedUnitCard = selectedDice.GetSelectedCard().GetCardDataSO();
            
            AddAllDiceDataToList(_selectedUnitCardDiceDataList, _selectedUnitCard);

            yield return StartCoroutine(MoveTargetUnitForClash(_selectedUnit, _targetUnit));

            while (_selectedUnitCardDiceDataList.Count > 0)
            {
                yield return StartCoroutine(StartOneSidedCardAttack(_selectedUnit, _targetUnit, _selectedUnitCardDiceDataList));
            }

            combatQueueList.Remove(selectedDice);
            selectedDice.ClearTargetDice();
            selectedDice.ClearSelectedCard();
        }*/

        yield return null;

        _selectedUnitCardDiceDataList.Clear();
        _targetUnitCardDiceDataList.Clear();
    }

    private IEnumerator StartOneSidedCardAttack(Unit attackerUnit, Unit targetUnit, List<CardToken> attackerUnitDiceDataList)
    {
        yield return new WaitForSeconds(1f);

        var attackerUnitDiceData = attackerUnitDiceDataList[0];
        int attackerUnitDiceValue = RollCardDice(attackerUnitDiceData);

        /*if (attackerUnitDiceData.isOffensiveDice)
        {
            targetUnit.TakeDamage(attackerUnitDiceValue);

            attackerUnitDiceDataList.RemoveAt(0);
        }
        else
        {
            attackerUnitDiceDataList.RemoveAt(0);
        }*/

    }

    private IEnumerator StartSingleCardDiceClash(CardToken selectedUnitDiceData, Unit selectedUnit, CardToken targetUnitDiceData, Unit targetUnit)
    {
        /*if (selectedUnitDiceData.Type != DiceData.DiceType.NotSet && targetUnitDiceData.Type != DiceData.DiceType.NotSet)
        {
            // Card dice clash

            yield return StartCoroutine(MoveTargetUnitForClash(selectedUnit, targetUnit));

            int selectedUnitDiceValue = RollCardDice(selectedUnitDiceData);
            int targetUnitDiceValue = RollCardDice(targetUnitDiceData);

            if (selectedUnitDiceValue > targetUnitDiceValue)
            {
                // Attacker win

                _attackerUnit = selectedUnit;
                _defenderUnit = targetUnit;
                _attackerDiceValue = selectedUnitDiceValue;
                _defenderDiceValue = targetUnitDiceValue;

                HandleCombatOutcome(selectedUnitDiceData, targetUnitDiceData, _selectedUnitCardDiceDataList, _targetUnitCardDiceDataList);

            }
            else if (selectedUnitDiceValue < targetUnitDiceValue)
            {
                // Defender win

                _attackerUnit = targetUnit;
                _defenderUnit = selectedUnit;
                _attackerDiceValue = targetUnitDiceValue;
                _defenderDiceValue = selectedUnitDiceValue;

                HandleCombatOutcome(targetUnitDiceData, selectedUnitDiceData, _selectedUnitCardDiceDataList, _targetUnitCardDiceDataList);
            }
            else
            {
                // Debug.Log("clash is draw");
            }

            yield return new WaitForSeconds(.5f);
        }
        else if (selectedUnitDiceData.Type != DiceData.DiceType.NotSet && targetUnitDiceData.Type == DiceData.DiceType.NotSet)
        {
            // One sided card dice attack from A to B

            yield return StartCoroutine(MoveTargetUnitForClash(targetUnit, selectedUnit));

            int diceValue = RollCardDice(selectedUnitDiceData);

            _attackerUnit = selectedUnit;
            _defenderUnit = targetUnit;
            _attackerDiceValue = diceValue;
        
            HandleOneSidedOutcome(selectedUnitDiceData, _selectedUnitCardDiceDataList, _targetUnitCardDiceDataList);
        }
        else if (selectedUnitDiceData.Type == DiceData.DiceType.NotSet && targetUnitDiceData.Type == DiceData.DiceType.NotSet)
        {
            // One sided card dice attack from B to A

            yield return StartCoroutine(MoveTargetUnitForClash(selectedUnit, targetUnit));

            int diceValue = RollCardDice(targetUnitDiceData);

            _attackerUnit = targetUnit;
            _defenderUnit = selectedUnit;
            _attackerDiceValue = diceValue;
        
            HandleOneSidedOutcome(targetUnitDiceData, _selectedUnitCardDiceDataList, _targetUnitCardDiceDataList);
        }
        else
        {
            // Both card dice has nothing
        }*/

        yield return null;
    }

    private void HandleOneSidedOutcome(CardToken attackerDiceData, List<CardToken> attackerDiceDataList, List<CardToken> defenderDiceDataList)
    {
        /*if (attackerDiceData.isOffensiveDice)
        {
            // attacker deal full damage 
            _defenderUnit.TakeDamage(_attackerDiceValue);

            // play animation
            // move unit

            attackerDiceDataList.RemoveAt(0);
            defenderDiceDataList.RemoveAt(0);
        }
        else if (attackerDiceData.Type == DiceData.DiceType.Guard)
        {
            // dice is negated

            // play animation
            // move unit

            attackerDiceDataList.RemoveAt(0);
            defenderDiceDataList.RemoveAt(0);
        }
        else if (attackerDiceData.Type == DiceData.DiceType.Evade)
        {
            // dice is negated

            // play animation
            // move unit

            attackerDiceDataList.RemoveAt(0);
            defenderDiceDataList.RemoveAt(0);
        }*/
    }

    private void HandleCombatOutcome(CardToken winnerDiceData, CardToken loserDiceData, List<CardToken> winnerDiceDataList, List<CardToken> loserDiceDataList)
    {
        Debug.Log("card dice clash");
        /*if (winnerDiceData.isOffensiveDice)
        {
            if (loserDiceData.isOffensiveDice)
            {
                // attacker deal full damage 
                _defenderUnit.TakeDamage(_attackerDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Guard)
            {
                // attacker deal substracted damage
                _defenderUnit.TakeDamage(_attackerDiceValue - _defenderDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Evade)
            {
                // attacker deal full damage
                _defenderUnit.TakeDamage(_attackerDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
        }
        else if (winnerDiceData.Type == DiceData.DiceType.Guard)
        {
            if (loserDiceData.isOffensiveDice)
            {
                // attacker deal substracted stagger damage
                _defenderUnit.TakeStaggerDamage(_attackerDiceValue - _defenderDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Guard)
            {
                // attacker deal full stagger damage
                _defenderUnit.TakeStaggerDamage(_attackerDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Evade)
            {
                // attacker deal full stagger damage
                _defenderUnit.TakeStaggerDamage(_attackerDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
        }
        else if (winnerDiceData.Type == DiceData.DiceType.Evade)
        {
            if (loserDiceData.isOffensiveDice)
            {
                // attacker recover stagger resist
                _attackerUnit.RecoverStaggerResist(_attackerDiceValue);

                // play animation
                // move unit

                // retain evade dice
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Guard)
            {
                // attacker recover stagger resist
                _attackerUnit.RecoverStaggerResist(_attackerDiceValue);

                // play animation
                // move unit

                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
            else if (loserDiceData.Type == DiceData.DiceType.Evade)
            {
                // both dice are negated
                winnerDiceDataList.RemoveAt(0);
                loserDiceDataList.RemoveAt(0);
            }
        }*/
    }

    private void AddAllDiceDataToList(List<CardToken> diceDataList, CardData cardDataSO)
    {
        /*diceDataList.Add(cardDataSO.firstDice);
        diceDataList.Add(cardDataSO.secondDice);
        diceDataList.Add(cardDataSO.thirdDice);*/
    }

    private IEnumerator MoveTargetUnitForClash(Unit movingUnit, Unit standbyUnit)
    {
        if (movingUnit.transform.position.x > standbyUnit.transform.position.x)
        {
            Vector3 movePosition = standbyUnit.transform.position + Vector3.right;

            //CameraMovement.Instance.MoveCameraToBetweenUnitClash(standbyUnit.transform.position, movePosition);
            yield return movingUnit.transform.DOMove(movePosition, .5f).SetEase(Ease.Linear).WaitForCompletion();
        }
        else
        {
            Vector3 movePosition = standbyUnit.transform.position + Vector3.left;

            //CameraMovement.Instance.MoveCameraToBetweenUnitClash(standbyUnit.transform.position, movePosition);
            yield return movingUnit.transform.DOMove(movePosition, .5f).SetEase(Ease.Linear).WaitForCompletion();
        }
    }

    private int RollCardDice(CardToken cardDice)
    {
        return UnityEngine.Random.Range(cardDice.MinValue, cardDice.MaxValue + 1);
    }

    private bool IsTargetingEachOther(UnitDice diceA, UnitDice diceB)
    {
        //return diceA.GetTargetDice() == diceB && diceB.GetTargetDice() == diceA;
        return true;
    }
    
}
