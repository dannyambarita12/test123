public abstract class TokenAction
{
    protected Unit _performer;
    protected Unit _target;
    protected int _diceAmount;
    protected Card _performerCard;
    protected Card _targetCard;

    public TokenAction(Unit attacker, Unit target, int diceAmount)
    {
        _performer = attacker;
        _target = target;
        _diceAmount = diceAmount;
    }

    public TokenAction(ClashResult clashResut)
    {
        _performer = clashResut.Winner.Unit;
        _target = clashResut.Target.Unit;
        _diceAmount = clashResut.DiceAmount;

        _performerCard = clashResut.Winner.Card;
        _targetCard = clashResut.Target.Card;
    }

    public abstract void Execute();
}


/*
 * RANGED and MELEE
 * SUPPORT and OFFENSE
 * PHYSICAL/MENTAL/MAGIC vs BLOCK
 * PHYSICAL/MENTAL/MAGIC vs DODGE
 * 
 * Akan ada visual dimana MELEE parry RANGED kalau melee token menang
 * Akan ada visual dimana ATTACK target Unit yang dituju kalau token yang menang adalah PHYSICAL/MENTAL/MAGIC ATTACK
 * Akan ada visual dimana SUPPORT target Unit yang dituju
 * Akan ada visual dimana ATTACK di BLOCK
 * Akan ada visual dimana ATTACK di DODGE
 */