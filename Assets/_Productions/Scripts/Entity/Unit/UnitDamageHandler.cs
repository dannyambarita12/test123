using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDamageHandler : MonoBehaviour
{
    
    [SerializeField] private Health health;
    [SerializeField] private UnitStagger stagger;

    private UnitResistanceData resistanceData;

    public void SetupResistanceData(UnitResistanceData resistanceData)
    {
        this.resistanceData = resistanceData;
    }
    
    public void TakeStaggerAndHealthDamage(DamageType damageType, int damageAmount)
    {
        int healthDamageAmount;
        int staggerDamageAmount;

        switch (damageType)
        {
            case DamageType.Slash:
                staggerDamageAmount = CalculateTotalDamage(resistanceData.slashStaggerResistance, damageAmount); 
                healthDamageAmount = CalculateTotalDamage(resistanceData.slashHealthResistance, damageAmount);
                stagger.DecreaseStagger(staggerDamageAmount);
                
                break;
            case DamageType.Pierce:
                staggerDamageAmount = CalculateTotalDamage(resistanceData.pierceStaggerResistance, damageAmount); 
                healthDamageAmount = CalculateTotalDamage(resistanceData.pierceHealthResistance, damageAmount);
                stagger.DecreaseStagger(staggerDamageAmount);
                
                break;
            case DamageType.Blunt:
                staggerDamageAmount = CalculateTotalDamage(resistanceData.bluntStaggerResistance, damageAmount); 
                healthDamageAmount = CalculateTotalDamage(resistanceData.bluntHealthResistance, damageAmount);
                stagger.DecreaseStagger(staggerDamageAmount);
                
                break;
        }
    }

    public void TakeFullHealthDamage(int damageAmount)
    {
        stagger.DecreaseStagger(damageAmount);
    }

    public void TakeFullStaggerDamage(int damageAmount)
    {
        stagger.DecreaseStagger(damageAmount);
    }

    private int CalculateTotalDamage(float resistanceValue, int damageAmount)
    {
        // Apply flat damage modifier

        // Apply multiply damage modifier
        return Mathf.RoundToInt(damageAmount * resistanceValue);
    }
    
}

public enum DamageType
{
    Slash = 0,
    Pierce = 1,
    Blunt = 2,
}

[System.Serializable]
public class UnitResistanceData
{
    public float slashHealthResistance;
    public float pierceHealthResistance;
    public float bluntHealthResistance;

    public float slashStaggerResistance;
    public float pierceStaggerResistance;
    public float bluntStaggerResistance;
}
