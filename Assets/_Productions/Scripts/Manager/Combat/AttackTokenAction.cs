using UnityEngine;

public class AttackTokenAction : TokenAction
{
    private CardTokenType _performerToken;
    private CardTokenType _targetToken;

    public AttackTokenAction(ClashResult clashResult) : base(clashResult)
    {
        _performerToken = clashResult.Winner.Token.Type;
        _targetToken = clashResult.Target.Token.Type;
    }

    public override void Execute()
    {
        Debug.Log($"Execute Attack Token Action from {_performer.name} to {_target.name} with dice amount {_diceAmount} and type {_performerToken}");

        // If performer is melee and target is ranged-attack, then parry the attack
        if (CardExtension.IsParry(_performerCard.CardData.DistanceType, _targetCard.CardData.DistanceType, _targetToken))
        {
            _performer.Health.SetInvulnerable(true, InvulnerableType.Parry);
            _target.PerformAttack(_performer.Health, 0);
        }
        else
        {
            _target.Health.SetInvulnerable(false);
            _performer.PerformAttack(_target.Health, _diceAmount);
        }
    }
}
