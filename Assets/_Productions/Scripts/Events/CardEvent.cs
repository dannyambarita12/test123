using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VadimskyiLab.Events;

public class CardEvent : EventBase<CardEvent>
{    
}

public class DiceSelectEvent: EventBase<DiceSelectEvent, DiceModel, bool>
{
}

public class DiceHoverEvent : EventBase<DiceHoverEvent, DiceModel, bool>
{
}

public class DiceEnemyHoverEvent: EventBase<DiceEnemyHoverEvent, DiceModel, bool>
{
}