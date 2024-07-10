using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct ClashResult
{
    public ClashData Winner;
    public ClashData Target;
    public bool IsDraw;
    public int DiceAmount;
}

public class ClashData
{
    public Unit Unit;
    public Card Card;
    public CardToken Token;
    public bool IsRemoveToken;
    public int DiceValue;
    public List<CardToken> TokenSequence;
    
    public CardDistanceType DistanceType => Card.CardData.DistanceType;
}

public class ClashHandler : MonoBehaviour
{
    private CombatData _activeCombatData;
    private List<CardToken> _activeTokenSequence = new();

    private CombatManager _combatManager;

    private ClashData _clashDataA;
    private ClashData _clashDataB;
    private CombatData _targetCombatData;
    private GameInput _gameInput;

    public void Init(CombatManager combatManager, GameInput gameInput)
    {
        _combatManager = combatManager;
        _gameInput = gameInput;
    }

    public void SetupClash(CombatData activeCombat)
    {
        _activeCombatData = activeCombat;
        _activeTokenSequence = _activeCombatData.UsedCard.Sequence.ToList();

        List<CardToken> targetSequence = new();

        if (_combatManager.HasCombatData(_activeCombatData.TargetDiceId, out _targetCombatData))
        {
            targetSequence = _targetCombatData.UsedCard.Sequence.ToList();
        }

        _clashDataA = new ClashData()
        {
            Unit = _activeCombatData.SourceUnit,
            Card = _activeCombatData.UsedCard,
            TokenSequence = _activeTokenSequence
        };

        _clashDataB = new ClashData()
        {
            Unit = _activeCombatData.TargetUnit,
            Card = _targetCombatData != null ? _targetCombatData.UsedCard : null,
            TokenSequence = targetSequence,
        };

        if (_targetCombatData != null)
            _combatManager.RemoveCombat(_targetCombatData);
    }

    public async UniTask Process()
    {
        ClashResult clashResult = default;
        var sequenceA = _clashDataA.TokenSequence;
        var sequenceB = _clashDataB.TokenSequence;

        bool isSourceUnitOnLeft = IsOnLeftTarget(_activeCombatData.SourceUnit.transform.position, _activeCombatData.TargetUnit.transform.position);
        _activeCombatData.SourceUnit.PrepareClash(_activeCombatData.UsedCard, isSourceUnitOnLeft);

        if (_targetCombatData != null)
        {
            bool isTargetOnLeft = IsOnLeftTarget(_activeCombatData.TargetUnit.transform.position, _activeCombatData.SourceUnit.transform.position);
            _activeCombatData.TargetUnit.PrepareClash(_targetCombatData.UsedCard, isTargetOnLeft);
        }

        while (AreBothClashingUnitsAlive() && (sequenceA.Count > 0 || sequenceB.Count > 0))
        {
            _clashDataA.Unit.UnitCombatUI?.SetupTokenSequence(sequenceA);
            _clashDataB.Unit.UnitCombatUI?.SetupTokenSequence(sequenceB);

            var tokenA = sequenceA.Count > 0 ? sequenceA[0] : default;
            var tokenB = sequenceB.Count > 0 ? sequenceB[0] : default;
            
            if (sequenceA.Count > 0 && sequenceB.Count > 0)
            {
                _clashDataA.Token = tokenA;
                _clashDataB.Token = tokenB;

                clashResult = SimulateClash(_clashDataA, _clashDataB);
                
                _clashDataA.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenA.MinValue}-{tokenA.MaxValue}");
                _clashDataB.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenB.MinValue}-{tokenB.MaxValue}");

                await MoveBothUnitForClash(_clashDataA.Unit, _clashDataB.Unit);
                await PlayClashVisual(clashResult);

                _clashDataA = clashResult.Winner;
                _clashDataB = clashResult.Target;

                sequenceA = _clashDataA.TokenSequence;
                sequenceB = _clashDataB.TokenSequence;

                if (_clashDataA.IsRemoveToken)
                    sequenceA.RemoveAt(0);

                if (_clashDataB.IsRemoveToken)
                    sequenceB.RemoveAt(0);
            }
            else if (sequenceA.Count > 0)
            {
                _clashDataA.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenA.MinValue}-{tokenA.MaxValue}");
                await MoveUnitToTarget(_clashDataA.Unit, _clashDataB.Unit);
                await PlayOneSideVisual(_clashDataA.Unit, _clashDataB.Unit, tokenA);
                sequenceA.RemoveAt(0);
            }
            else if (sequenceB.Count > 0)
            {
                _clashDataB.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenB.MinValue}-{tokenB.MaxValue}");
                await MoveUnitToTarget(_clashDataB.Unit, _clashDataA.Unit);
                await PlayOneSideVisual(_clashDataB.Unit, _clashDataA.Unit, tokenB);
                sequenceB.RemoveAt(0);
            }

            await UniTask.Delay(500);

            _clashDataA.Unit.RefreshClash();
            _clashDataB.Unit.RefreshClash();

            clashResult = default;
        }

        _activeCombatData = null;
        _targetCombatData = null;

        _clashDataA.Unit.SetCombatUI(false, false);
        _clashDataB.Unit.SetCombatUI(false, false);
    }

    private ClashResult SimulateClash(ClashData clashDataA, ClashData clashDataB)
    {
        int diceValueA = DiceHelper.Roll(clashDataA.Token.MinValue, clashDataA.Token.MaxValue);
        int diceValueB = DiceHelper.Roll(clashDataB.Token.MinValue, clashDataB.Token.MaxValue);

        clashDataA.DiceValue = diceValueA;
        clashDataB.DiceValue = diceValueB;

        var clashResult = new ClashResult()
        {
            Winner = diceValueA > diceValueB ? clashDataA : clashDataB,
            Target = diceValueA > diceValueB ? clashDataB : clashDataA,
            IsDraw = diceValueA == diceValueB,
            DiceAmount = Mathf.Max(diceValueA, diceValueB),
        };

        bool isParry = CardExtension.IsParry(clashResult.Winner.DistanceType, clashResult.Target.DistanceType, clashResult.Target.Token.Type);

        if (isParry)
        {
            clashResult.Winner.IsRemoveToken = false;
        }
        else if (clashResult.IsDraw)
        {
            clashResult.Winner.IsRemoveToken = true;
        }
        else
        {
            clashResult.Winner.IsRemoveToken = clashResult.Winner.Token.Type != CardTokenType.Dodge;
        }

        clashResult.Target.IsRemoveToken = true;

        return clashResult;
    }

    private IEnumerator PlayOneSideVisual(Unit sourceUnit, Unit targetUnit, CardToken token)
    {
        sourceUnit.UnitCombatUI.SetDiceEstimateText($"{token.MinValue}-{token.MaxValue}");
        targetUnit.SetCombatUI(false, false);

        yield return new WaitUntil(() =>
        {
            return _gameInput.AutoCombat || Input.GetKeyUp(KeyCode.Space);
        });

        int diceValue = DiceHelper.Roll(token.MinValue, token.MaxValue);
        sourceUnit.UnitCombatUI.SetFinalDiceAmount(diceValue.ToString());

        Debug.Log($"{sourceUnit.name} attack {targetUnit.name} with token : {token.Type} - {diceValue}");

        if (token.Type == CardTokenType.Block || token.Type == CardTokenType.Dodge)
            yield break;

        targetUnit.Health.SetInvulnerable(false);
        sourceUnit.IsFinishAction = false;
        sourceUnit.PerformAttack(targetUnit.Health, diceValue);

        yield return new WaitForSeconds(1f);
    }

    private async UniTask PlayClashVisual(ClashResult clashResult)
    {
        var winner = clashResult.Winner;
        var target = clashResult.Target;
        var tokenA = winner.Token;
        var tokenB = target.Token;
        var diceValueA = winner.DiceValue;
        var diceValueB = target.DiceValue;

        /*winner.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenA.MinValue}-{tokenA.MaxValue}");
        target.Unit.UnitCombatUI.SetDiceEstimateText($"{tokenB.MinValue}-{tokenB.MaxValue}");*/

        await UniTask.WaitUntil(() =>
        {
            return _gameInput.AutoCombat || Input.GetKeyUp(KeyCode.Space);
        });

        winner.Unit.UnitCombatUI?.SetFinalDiceAmount(diceValueA.ToString());
        target.Unit.UnitCombatUI?.SetFinalDiceAmount(diceValueB.ToString());

        if (clashResult.IsDraw)
        {
            Debug.Log("DRAW");
            return;
        }

        if (winner.Token.Type.IsAttack() == false && target.Token.Type.IsAttack() == false)
        {
            Debug.Log("Both are not attack token");
            return; 
        }

        winner.Unit.IsFinishAction = false;

        //TODO:: Sepertinya bisa dibuat lebih baik lagi
        TokenAction tokenAction = null;
        switch (winner.Token.Type)
        {
            case CardTokenType.Physical:
            case CardTokenType.Magical:
            case CardTokenType.Mental:
                tokenAction = new AttackTokenAction(clashResult);
                break;
            case CardTokenType.Block:
                tokenAction = new BlockTokenAction(clashResult);
                break;
            case CardTokenType.Dodge:
                tokenAction = new DodgeTokenAction(clashResult);
                break;
        }

        if (tokenAction != null)
            tokenAction.Execute();

        if (winner.Token.Type.IsAttack() || target.Token.Type.IsAttack())
        {
            /*await UniTask.WaitUntil(() =>
            {
                return winner.Unit.IsFinishAction || target.Unit.IsFinishAction;
            });*/

            await UniTask.Delay(1000);
        }
    }

    private IEnumerator MoveBothUnitForClash(Unit unitA, Unit unitB)
    {
        if (unitA.transform.position.x > unitB.transform.position.x)
        {
            var interpolant = Random.Range(.1f, .9f);
            var meetingPoint = Vector3.Lerp(unitA.transform.position, unitB.transform.position, interpolant);
            unitA.transform.DOMove(meetingPoint + (Vector3.right * 2f), 0.5f);
            unitB.transform.DOMove(meetingPoint + (Vector3.left * 2f), 0.5f);
        }
        else
        {
            var interpolant = Random.Range(.05f, .95f);
            var meetingPoint = Vector3.Lerp(unitA.transform.position, unitB.transform.position, interpolant);
            unitA.transform.DOMove(meetingPoint + (Vector3.left * 2f), 0.5f);
            unitB.transform.DOMove(meetingPoint + (Vector3.right * 2f), 0.5f);
        }

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveUnitToTarget(Unit unit, Unit target)
    {
        var isOnLeft = IsOnLeftTarget(unit.transform.position, target.transform.position);
        var targetPoint = target.transform.position + (isOnLeft ? Vector3.left : Vector3.right) * 4f;
        unit.transform.DOMove(targetPoint, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    private bool AreBothClashingUnitsAlive()
    {
        return _clashDataA.Unit.Health.IsAlive && _clashDataB.Unit.Health.IsAlive;
    }

    private bool IsOnLeftTarget(Vector3 posA, Vector3 targetPos)
    {
        return posA.x < targetPos.x;
    }
}

public static class TokenBehaviour
{
    public static void ExecuteBlockToken(this CardTokenType tokenType, Unit unit)
    {
        if(tokenType != CardTokenType.Block)
            return;

        unit.Health.SetInvulnerable(true, InvulnerableType.Block);
    }

    public static void ExecuteDodgeToken(this CardTokenType tokenType, Unit unit)
    {
        if(tokenType != CardTokenType.Dodge)
            return;

        unit.Health.SetInvulnerable(true, InvulnerableType.Dodge);
    }
}
