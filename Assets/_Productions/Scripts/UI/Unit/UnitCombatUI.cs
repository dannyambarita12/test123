using CustomExtensions;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCombatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cardNameText;

    [Header("Dice")]
    [SerializeField]
    private TextMeshProUGUI diceAmountText;
    [SerializeField]
    private TextMeshProUGUI diceEstimateText;

    [Header("Token")]
    [SerializeField]
    private Image[] tokenImages;
    [SerializeField]
    private TokenImageDatabase tokenImageDatabase;

    public void SetCardName(string cardName)
    {
        cardNameText.text = cardName;
    }

    public void SetFinalDiceAmount(string diceAmount)
    {
        diceAmountText.text = diceAmount;
        diceEstimateText.text = string.Empty;
    }

    public void SetDiceEstimateText(string diceAmount)
    {
        diceEstimateText.text = diceAmount;
        diceAmountText.text = "?";
    }    

    public void ShowUI(bool condition)
    {
        gameObject.SetActive(condition);
    }    

    public void SetupTokenSequence(List<CardToken> sequence)
    {
        tokenImages.ForEach(x => x.SetActive(false));
        for (int i = 0; i < sequence.Count; i++)
        {
            if (i >= tokenImages.Length)
            {
                Debug.LogWarning("Enemy Token Image is not enough");
                break;
            }

            tokenImages[i].sprite = tokenImageDatabase.GetTokenImage(sequence[i].Type).tokenSprite;
            tokenImages[i].SetActive(true);
        }
    }
}
