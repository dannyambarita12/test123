using DependencyInjection;
using UnityEngine;

public class EnemyCombatSetup : MonoBehaviour
{
    [SerializeField]
    private PlayerPhaseManager cardInputManager;
    [SerializeField]
    private UnitManager unitManager;

    [Inject]
    private GameStateManager _gameStateManager;

    private void Start()
    {
        _gameStateManager[GameState.EnemySetup].onEnter += SetupEnemyTarget;
    }

    private void SetupEnemyTarget(GameState state)
    {
        var enemies = unitManager.Enemies;

        foreach (var sourceUnit in enemies)
        {            
            var sourceDiceModel = new DiceModel();
            var targetDiceModel = new DiceModel();
            Card selectedCard = sourceUnit.CardHandler.GetRandomCard();

            foreach(var sourceDice in sourceUnit.CardHandler.ActiveDices)
            {
                if (sourceDice.IsActive == false)
                    continue;

                var targetUnit = unitManager.GetRandomPlayerUnit();
                var targetDice = targetUnit.CardHandler.GetRandomDice();

                sourceDiceModel.Unit = sourceUnit;
                sourceDiceModel.SelectedDice = sourceDice;                
                targetDiceModel.Unit = targetUnit;
                targetDiceModel.SelectedDice = targetDice;
            }

            if(cardInputManager != null)
                cardInputManager.CreateCombatData(sourceDiceModel, targetDiceModel, selectedCard);
        }

        _gameStateManager.ChangeState(GameState.PlayerSetup);
    }
}
