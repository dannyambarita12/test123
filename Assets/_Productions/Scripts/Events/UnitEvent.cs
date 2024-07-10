using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VadimskyiLab.Events;

public class UnitEvent : EventBase<UnitEvent, Unit, bool>
{
}

public class UnitHoverEvent : EventBase<UnitHoverEvent, Unit, bool>
{
}

public class UnitSelectedEvent : EventBase<UnitSelectedEvent, bool>
{
}