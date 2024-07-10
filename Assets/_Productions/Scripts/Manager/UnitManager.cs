using CustomExtensions;
using DependencyInjection;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Unit> Players => playerUnitList;
    public List<Unit> Enemies => enemyUnitList;

    [Title("Unit Spawn List")]
    [SerializeField]
    private Transform[] playerSpawnPositions;
    [SerializeField]
    private Transform[] enemySpawnPositions;

    [Title("Units")]
    [SerializeField]
    private List<Unit> playerUnitList = new();
    [SerializeField]
    private List<Unit> enemyUnitList = new();

    [Title("Dead Units")]
    [SerializeField]
    private List<Unit> deadPlayerUnits = new ();
    [SerializeField]
    private List<Unit> deadEnemyUnits = new ();

    [Title("Debug Initial Unit")]
    [SerializeField]
    private Unit[] debugPlayerUnits;
    [SerializeField]
    private Unit[] debugEnemyUnits;

    [Inject]
    private GameStateManager _gameStateManager;

    private void Start()
    {
        _gameStateManager[GameState.Initialize].onEnter += InitializeUnit;
        _gameStateManager[GameState.RoundStart].onEnter += OnRoundStart;
        _gameStateManager[GameState.RollDice].onEnter += RollUnitDice;
        _gameStateManager[GameState.Combat].onEnter += OnCombatBegin;
        _gameStateManager[GameState.Combat].onExit += OnCombatEnd;
    }

    public Unit GetRandomPlayerUnit()
    {
        return Players[UnityEngine.Random.Range(0, Players.Count)];
    }

    private async void InitializeUnit(GameState state)
    {
        //TODO: Initialize Stage Unit
        //Waiting initialization unit

        for (int i = 0; i < playerSpawnPositions.Length; i++)
        {
            if(i >= debugPlayerUnits.Length)
            {
                Debug.LogWarning("Player Unit is not enough");
                break;
            }

            var unit = Instantiate(debugPlayerUnits[i], playerSpawnPositions[i].position, Quaternion.identity);
            playerUnitList.Add(unit);
        }

        for (int i = 0; i < enemySpawnPositions.Length; i++)
        {
            if (i >= debugEnemyUnits.Length)
            {
                Debug.LogWarning("Enemy Unit is not enough");
                break;
            }

            var unit = Instantiate(debugEnemyUnits[i], enemySpawnPositions[i].position, Quaternion.identity);
            enemyUnitList.Add(unit);
        }

        foreach (var unit in playerUnitList)
        {
            unit.Health.OnDeath.AddListener(RemoveDeadPlayerUnit);
        }

        foreach (var unit in enemyUnitList)
        {
            unit.Health.OnDeath.AddListener(RemoveDeadEnemyUnit);
        }

        await Task.Delay(1000);

        Debug.Log("Initialize Unit");

        _gameStateManager.NextState();
    }

    private void OnRoundStart(GameState state)
    {
        var units = new List<Unit>();
        units.AddRange(playerUnitList);
        units.AddRange(enemyUnitList);

        foreach (Unit unit in units)
            unit.CardHandler.PrepareUnit();        
    }

    private void RollUnitDice(GameState state)
    {
        foreach (Unit playerUnit in playerUnitList)
        {
            playerUnit.CardHandler.RollDice();
        }

        foreach (Unit enemyUnit in enemyUnitList)
        {
            enemyUnit.CardHandler.RollDice();
        }

        _gameStateManager.ChangeState(GameState.EnemySetup);
    }

    private void OnCombatBegin(GameState state)
    {
        var units = new List<Unit>();
        units.AddRange(playerUnitList);
        units.AddRange(enemyUnitList);

        foreach (Unit unit in units)
            unit.BeginCombatPhase();
    }

    private void OnCombatEnd(GameState state)
    {
        var units = new List<Unit>();
        units.AddRange(playerUnitList);
        units.AddRange(enemyUnitList);

        foreach (Unit unit in units)
            unit.EndCombatPhase();
    }

    private void RemoveDeadPlayerUnit(Unit deadUnit)
    {
        playerUnitList.Remove(deadUnit);
        deadPlayerUnits.Add(deadUnit);

        deadUnit.SetActive(false);
    }

    private void RemoveDeadEnemyUnit(Unit deadUnit)
    {
        enemyUnitList.Remove(deadUnit);
        deadEnemyUnits.Add(deadUnit);

        deadUnit.SetActive(false);
    }
}
