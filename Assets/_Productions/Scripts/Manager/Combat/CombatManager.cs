using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using DependencyInjection;
using System.Linq;
using Cysharp.Threading.Tasks;
using Lean.Pool;

[RequireComponent(typeof(ClashHandler))]
public class CombatManager : MonoBehaviour, IDependencyProvider
{
    [Header("Combat Data")]
    [ShowInInspector]
    private List<CombatData> combatQueue = new();

    [ShowInInspector]
    [ReadOnly]
    private List<CombatData> sortedQueue = new List<CombatData>();

    [Header("Handlers")]
    [SerializeField]
    private ClashHandler clashHandler;

    [Header("Camera")]
    [SerializeField]
    private CameraMovement cameraMovement;

    [Provide]
    private CombatManager ProvideDependency() => this;

    [Inject]
    private GameStateManager _gameStateManager;

    [Inject]
    private GameInput _gameInput;

    private void Awake()
    {
        clashHandler = GetComponent<ClashHandler>();
        clashHandler.Init(this, _gameInput);
    }

    private void Start()
    {
        if (_gameStateManager != null)
        {
            _gameStateManager[GameState.Combat].onEnter += OnCombatPhaseStart;
        }
    }

    public void RequestCombat(CombatData data)
    {
        combatQueue.Add(data);
    }

    public void RemoveCombat(CombatData data)
    {
        combatQueue.Remove(data);
    }

    public bool HasCombatData(string diceId, out CombatData data)
    {
        data = GetCombatDataById(diceId);
        return data != null;
    }

    private CombatData GetCombatDataById(string diceId)
    {
        for (int i = 0; i < combatQueue.Count; i++)
        {
            if (combatQueue[i].SourceDiceId == diceId)
            {
                return combatQueue[i];
            }
        }

        return null;
    }

    private void OnCombatPhaseStart(GameState state)
    {
        Debug.Log("Begin combat phase");
        RemoveTrajectoryTarget();
        SetupCombatQueue();

        UniTask.Void(ProcessCombatQueueCoroutine);
    }

    private void RemoveTrajectoryTarget()
    {
        for (int i = 0; i < combatQueue.Count; i++)
        {
            var data = combatQueue[i];
            LeanPool.Despawn(data.TrajectoryDrawer);
        }
    }

    private void SetupCombatQueue()
    {
        var sortedQueue = combatQueue.OrderByDescending(c => c.UsedCard.CardData.DistanceType == CardDistanceType.Ranged)
            .ThenByDescending(combatQueue => combatQueue.DiceValue).ToList();
        combatQueue = sortedQueue;
        this.sortedQueue = sortedQueue;
    }

    private async UniTaskVoid ProcessCombatQueueCoroutine()
    {
        await UniTask.Delay(500);

        while (combatQueue.Count > 0)
        {
            if (combatQueue.Count == 0)
                break;            

            CombatData sourceCombatData = combatQueue[0];
            cameraMovement.SetTarget(sourceCombatData.SourceUnit.transform, sourceCombatData.TargetUnit.transform);
            cameraMovement.ActivateCombatCamera(true);
            
            Debug.Log($"Begin Duel of {sourceCombatData.SourceUnit.name} vs {sourceCombatData.TargetUnit.name}");

            // First we highlight unit yang lagi pake kartu dan targetnya
            sourceCombatData.SourceUnit.HightlightUnitCombat();
            sourceCombatData.TargetUnit.HightlightUnitCombat();            
           
            switch (sourceCombatData.UsedCard.CardData.ActionType)
            {
                case CardActionType.Support:
                    Debug.Log("Perform Support Action");
                    break;

                case CardActionType.Attack:
                    clashHandler.SetupClash(sourceCombatData);
                    await clashHandler.Process();                    
                    break;                
            }

            Debug.Log($"End Card of {sourceCombatData.SourceUnit.name} vs {sourceCombatData.TargetUnit.name}");

            combatQueue.RemoveAt(0);

            // Mau ada target atau tidak, ttp aja kita harus dehighlight dan tutup UI
            sourceCombatData.SourceUnit.DeHighlightUnitCombat();
            sourceCombatData.TargetUnit.DeHighlightUnitCombat();
        }

        cameraMovement.ActivateCombatCamera(false);
        _gameStateManager.ChangeState(GameState.RoundStart);
    }
}

public static class DiceHelper
{
    public static int Roll(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue + 1);
    }
}