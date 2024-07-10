using CustomExtensions;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatView : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    [SerializeField]
    private TextMeshProUGUI playerCardNameText;
    [SerializeField]
    private TextMeshProUGUI playerCardDistanceText;
    [SerializeField]
    private Image[] tokenImages;

    [Header("Enemy")]
    [SerializeField]
    private TextMeshProUGUI enemyNameText;
    [SerializeField]
    private TextMeshProUGUI enemyCardNameText;
    [SerializeField]
    private TextMeshProUGUI enemyCardDistanceText;
    [SerializeField]
    private Image[] enemyTokenImages;

    [Header("Misc")]
    [SerializeField]
    private TextMeshProUGUI clashConditionText;
    [SerializeField]
    private TokenImageDatabase tokenImageDatabase;

    public void SetupUnitPlayer(Unit unit, Card card)
    {
        playerNameText.text = unit.UnitData.Name;

        if(card == null)
        {
            Debug.LogWarning("Player Card is null");
            return;
        }

        playerCardNameText.text = card.CardData.Name;
        playerCardDistanceText.text = card.CardData.DistanceType.ToString();

        SetupPlayerCardSequence(card.Sequence);
    }

    public void SetupUnitEnemy(Unit unit, Card card)
    {
        enemyNameText.text = unit.UnitData.Name;

        if(card == null)
        {
            Debug.LogWarning("Enemy Card is null");
            return;
        }

        enemyCardNameText.text = card.CardData.Name;
        enemyCardDistanceText.text = card.CardData.DistanceType.ToString();

        SetupEnemyCardSequence(card.Sequence);
    }

    public void SetupPlayerCardSequence(List<CardToken> sequence)
    {
        tokenImages.ForEach(x => x.SetActive(false));
        for (int i = 0; i < sequence.Count; i++)
        {
            if (i >= tokenImages.Length)
            {
                Debug.LogWarning("Token Image is not enough");
                break;
            }

            tokenImages[i].sprite = tokenImageDatabase.GetTokenImage(sequence[i].Type).tokenSprite;
            tokenImages[i].SetActive(true);
        }
    }

    public void SetupEnemyCardSequence(List<CardToken> sequence)
    {
        enemyTokenImages.ForEach(x => x.SetActive(false));
        for (int i = 0; i < sequence.Count; i++)
        {
            if (i >= enemyTokenImages.Length)
            {
                Debug.LogWarning("Enemy Token Image is not enough");
                break;
            }

            enemyTokenImages[i].sprite = tokenImageDatabase.GetTokenImage(sequence[i].Type).tokenSprite;
            enemyTokenImages[i].SetActive(true);
        }
    }

    public void ClearUnitPlayer()
    {
        playerNameText.text = "";
        playerCardNameText.text = "";
        playerCardDistanceText.text = "";
        tokenImages.ForEach(x => x.SetActive(false));
    }

    public void ClearUnitEnemy()
    {
        enemyNameText.text = "";
        enemyCardNameText.text = "";
        enemyCardDistanceText.text = "";
        enemyTokenImages.ForEach(x => x.SetActive(false));
    }

    public void SetClashConditionText(string text)
    {
        clashConditionText.text = text;
    }
}
