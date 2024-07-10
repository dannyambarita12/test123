using UnityEngine;

public class BlockTokenAction : TokenAction
{
    private CardDistanceType _distanceType;
    private CardDistanceType _targetDistanceType;

    public BlockTokenAction(ClashResult clashResult) : base(clashResult)
    {
        _distanceType = clashResult.Winner.Card.CardData.DistanceType;
        _targetDistanceType = clashResult.Target.Card.CardData.DistanceType;
    }

    public override void Execute()
    {
        _performer.Health.SetInvulnerable(true, InvulnerableType.Block);
        _target.PerformAttack(_performer.Health, 0);
    }
}
