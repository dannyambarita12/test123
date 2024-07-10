using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Health : MonoBehaviour
{    
    public bool IsAlive => CurrentValue > 0;
    public int MaxValue => maxHealth;
    public int CurrentValue {
        get => currentHealth;
        private set
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnChange?.Invoke(currentHealth, maxHealth);
        }
    }

    [Title("Health Settings")]
    [SerializeField] 
    private int maxHealth = 50;
    [SerializeField] 
    private int currentHealth;

    [Title("Hit Settings")]
    [SerializeField]
    private HealthStatus healthStatus;
    [SerializeField]
    private InvulnerableType invulnerableType;

    [TitleGroup("Events"), PropertyOrder(5)]
    public UnityEvent<HitData> OnHit = new();
    [TitleGroup("Events"), PropertyOrder(5)]
    public UnityEvent<Unit> OnDeath = new();
    [TitleGroup("Events"), PropertyOrder(5)]
    public UnityEvent<float, float> OnChange = new();

    private void Start()
    {
        ResetHealthToMaximum();
    }

    public void ResetHealthToMaximum()
    {
        CurrentValue = maxHealth;
    }
    
    public void SetInvulnerable(bool isInvulnerable, InvulnerableType type = InvulnerableType.Block)
    {
        healthStatus = isInvulnerable ? HealthStatus.Invulnerable : HealthStatus.Normal;
        invulnerableType = type;
    }

    public void SetMaximumHealth(int amount, bool reset = false)
    {
        maxHealth = amount;

        if (reset)
            ResetHealthToMaximum();
    }

    public void ProcessHit(HitData hitData)
    {
        if (IsAlive == false) 
            return;

        if (hitData.HitType == HitType.Damage)
        {
            ApplyHit(ref hitData);
        }
        else if (hitData.HitType == HitType.Heal)
        {
            IncreaseHealth(hitData);
        }

        OnHit?.Invoke(hitData);

        if (CurrentValue <= 0)
        {
            Unit unit = GetComponent<Unit>();
            OnDeath?.Invoke(unit);
            Debug.Log($"{unit.name} is dead!");
        }
    }

    public void SetCurrentHealth(int amount)
    {
        CurrentValue = amount;
    }

    private void ApplyHit(ref HitData hitData)
    {
        hitData.IsInvulnerable = IsInvulnerable();

        if (IsInvulnerable())
        {
            Debug.Log($"Can't hit unit because invulnerable : {invulnerableType}");
            hitData.Amount = 0;
            hitData.InvulnerableType = invulnerableType;
            return;
        }

        CurrentValue -= hitData.Amount;
    }

    private void IncreaseHealth(HitData hitData)
    {
        CurrentValue += hitData.Amount;        
    }

    private bool IsInvulnerable()
    {
        return healthStatus == HealthStatus.Invulnerable;
    }

}

public enum HealthStatus
{
    Normal = 0,
    Invulnerable = 1,
}

public enum InvulnerableType
{
    Parry = 0, 
    Dodge = 1, 
    Block = 2,
    Immortal = 3,
    Immune = 4,
}

public struct HitData
{
    public int Amount;
    public HitType HitType;
    public Health Instigator;
    public Health Target;
    public bool IsCritical;
    public bool IsInvulnerable;
    public InvulnerableType InvulnerableType;
}

public enum HitType
{
    Damage = 0,
    Heal = 1,
}