using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDiceManager : MonoBehaviour
{
    [Serializable]
    public struct DiceTooltipUI
    {
        public DiceModel DiceModel;
        public CardDiceUI CardDiceUI;
    }

    [SerializeField]
    private CardDiceUI cardDiceUI;

    private List<DiceTooltipUI> _currentHoveredTooltip = new();
    private List<DiceTooltipUI> _currentSelectedTooltip = new();


    private void Awake()
    {
        DiceHoverEvent.Subscribe(OnDiceHover);
        DiceSelectEvent.Subscribe(OnDiceSelect);
    }

    private void OnDiceSelect(DiceModel model, bool isPlayer)
    {
        bool isSameUnit = _currentSelectedTooltip.Exists(x => x.DiceModel.Unit == model.Unit);
        bool isSameDice = _currentSelectedTooltip.Exists(x => x.DiceModel.SelectedDice == model.SelectedDice);

        if (isSameUnit)
        {
            var diceTooltip = _currentSelectedTooltip.Find(x => x.DiceModel.Unit == model.Unit);

            if (diceTooltip.CardDiceUI != null)
            {
                diceTooltip.CardDiceUI.ExpandCard(false);
                LeanPool.Despawn(diceTooltip.CardDiceUI);
                _currentSelectedTooltip.Remove(diceTooltip);
            }

            if (isSameDice)
                return;

            var tooltip = LeanPool.Spawn(cardDiceUI, model.SelectedDice.transform.position + Vector3.up, Quaternion.identity);
            diceTooltip = new DiceTooltipUI
            {
                DiceModel = model,
                CardDiceUI = tooltip
            };

            tooltip.ExpandCard(true);
            _currentSelectedTooltip.Add(diceTooltip);
        }
        else
        {
            var tooltip = LeanPool.Spawn(cardDiceUI, model.SelectedDice.transform.position + Vector3.up, Quaternion.identity);

            var diceTooltipUI = new DiceTooltipUI
            {
                DiceModel = model,
                CardDiceUI = tooltip
            };

            // TODO :: GET CARD BY SELECTED DICE
            // tooltip.SetCard(card);

            tooltip.ExpandCard(true);
            _currentSelectedTooltip.Add(diceTooltipUI);
        }
    }

    private void OnDiceHover(DiceModel model, bool isHover)
    {
        
    }
}
