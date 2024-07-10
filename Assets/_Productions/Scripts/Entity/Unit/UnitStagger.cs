using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class UnitStagger : MonoBehaviour
{
    [Header("Stagger Attribute")]
    [SerializeField] private int maxSP = 25;
    [SerializeField] private int currentSP;

    public int MaxHP => maxSP;
    public int CurrentHP => currentSP;

    public Action OnStaggerChanged;
    public Action OnStaggerDepleted;

    private void Start()
    {
        ResetStaggerToMaximum();
    }

    [Button]
    public void ResetStaggerToMaximum()
    {
        currentSP = maxSP;

        OnStaggerChanged?.Invoke();
    }

    public void SetMaximumStagger(int amount)
    {
        maxSP = amount;

        OnStaggerChanged?.Invoke();
    }

    public void SetCurrentStagger(int amount)
    {
        currentSP = amount;
        
        OnStaggerChanged?.Invoke();
    }

    public void IncreaseStagger(int amount)
    {
        currentSP += amount;

        OnStaggerChanged?.Invoke();
    }

    public void DecreaseStagger(int amount)
    {
        currentSP -= amount;

        if (currentSP <= 0)
        {
            OnStaggerDepleted?.Invoke();
        }

        OnStaggerChanged?.Invoke();
    }
}
