using Sirenix.OdinInspector;
using System;

[Serializable]
public struct BaseStatValue<T> where T : Enum
{
    [HideLabel]
    [HorizontalGroup("Base Stats")]
    public T Type;
    [HideLabel]
    [HorizontalGroup("Base Stats", Width = 0.25f)]
    public float Value;

    public BaseStatValue(T type, float value)
    {
        Type = type;
        Value = value;
    }
}
