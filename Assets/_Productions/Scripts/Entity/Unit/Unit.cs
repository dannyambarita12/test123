using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Lean.Pool;

public class Unit : MonoBehaviour
{
    public UnitCardHandler CardHandler => cardHandler;
    public UnitCombatUI UnitCombatUI => _currentCombatUI;
    public UnitData UnitData => unitData;
    public UnitStat Stat => unitStat;
    public Health Health => health;
    public bool IsPlayer => isPlayer;
    public bool IsFinishAction { get; set; }

    [Title("Unit Data")]
    [SerializeField] 
    private UnitData unitData;

    [Title("Unit Components")]
    [SerializeField] 
    private UnitCardHandler cardHandler;
    [SerializeField] 
    private Health health;
    [SerializeField]
    private UnitVisual unitVisual;
    [SerializeField]
    private UnitStat unitStat;
    [SerializeField]
    private UnitAnimator unitAnimator;

    [Title("Unit Settings")]
    [SerializeField] 
    private bool isPlayer;

    [Title("UNIT COMBAT UI")]
    [SerializeField]
    private Transform unitCombatUIPoint;
    [SerializeField]
    private UnitCombatUI leftCombatUI;
    [SerializeField]
    private UnitCombatUI rightCombatUI;

    public UnityEvent OnCombatPhaseBegin = new();
    public UnityEvent OnCombatPhaseEnd = new(); 
    public UnityEvent OnHighlightedCombat = new();
    public UnityEvent OnDehighlightedCombat = new();

    private Vector3 _startingPosition;
    private UnitCombatUI _currentCombatUI;

    private void Start()
    {
        _startingPosition = transform.position;
        OnCombatPhaseEnd.AddListener(BackToStartingPosition);
        InitializeUnit();
    }

    private void InitializeUnit()
    {
        //TODO:: SETUP LEVEL
        unitStat.InitStats(1, unitData.StrValue, unitData.DexValue, unitData.IntValue);
        cardHandler.Initialize(this);
        health.SetMaximumHealth(unitStat.Health, true);
        unitVisual.TurnUnit(isPlayer);
    }

    public void RefreshClash()
    {
        unitAnimator.ChangeAnimation(AnimationState.Clash);
    }

    public void SelectUnit()
    {
        UnitSelectedEvent.Invoke(isPlayer);
    }

    public void BeginCombatPhase()
    {
        unitVisual.DarkenUnit();
        unitAnimator.ChangeAnimation(AnimationState.Clash);
        SlightMoveForCombat();
        OnCombatPhaseBegin?.Invoke();
    }

    public void EndCombatPhase()
    {
        // TODO:: Bisa balikin ke posisi semula
        unitAnimator.PlayIdle();
        unitVisual.HighlightUnit();
        unitVisual.TurnUnit(isPlayer);
        SetCombatUI(false, false);
        OnCombatPhaseEnd?.Invoke();
    }

    //When unit start clashing
    public void PrepareClash(Card card, bool isLeftPosition)
    {
        unitVisual.TurnUnit(isLeftPosition);
        SetCombatUI(card != null, isLeftPosition);
    }

    // When Unit is targeted
    public void HightlightUnitCombat()
    {
        //unitAnimator.Animator.SetBool("IsClash", true);
        unitVisual.HighlightUnit();
        health.SetInvulnerable(false);

        OnHighlightedCombat?.Invoke();
    }

    public void DeHighlightUnitCombat()
    {
        unitVisual.DarkenUnit();        
        OnDehighlightedCombat?.Invoke();
    }

    public void SetCombatUI(bool condition, bool isLeft)
    {
        if (condition == false)
        {
            if(_currentCombatUI != null)
                LeanPool.Despawn(_currentCombatUI);

            _currentCombatUI = null;
            return;
        }

        var uiPrefab = isLeft ? leftCombatUI : rightCombatUI;
        _currentCombatUI = LeanPool.Spawn(uiPrefab, unitCombatUIPoint);
    }

    public void PerformAttack(Health target, int diceAmount)
    {
        Debug.Log($"{name} is attacking {target.name} with {diceAmount} dice");

        unitAnimator.Animator.PlayRandom(true);

        var hitData = new HitData()
        {
            Amount = diceAmount,
            HitType = HitType.Damage,
            Instigator = Health,
            Target = target,
            IsCritical = false
        };

        target.ProcessHit(hitData);
    }

    public void FinishAttackAnimation()
    {
        IsFinishAction = true;
    }

    public Vector2 GetUnitDirection(float comparerPosition)
    {
        if (transform.position.x < comparerPosition)
            return Vector2.right;
        else
            return Vector2.left;
    }

    private void SlightMoveForCombat()
    {
        var dir = GetUnitDirection(0);
        transform.DOMove(transform.position + (Vector3)dir * 1.5f, 0.25f);
    }

    private void BackToStartingPosition()
    {
        transform.DOMove(_startingPosition, 0.5f);
    }

    private void OnMouseEnter()
    {
        UnitHoverEvent.Invoke(this, true);
    }

    private void OnMouseExit()
    {
        UnitHoverEvent.Invoke(this, false);        
    }    
}
