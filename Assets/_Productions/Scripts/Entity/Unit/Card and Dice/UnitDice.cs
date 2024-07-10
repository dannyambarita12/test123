using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitDice : MonoBehaviour
{
    public string DiceId => _diceId;
    public int DiceValue => _diceValue;
    public bool IsActive => _isActive;

    [SerializeField] 
    private TextMeshProUGUI diceText;
    [SerializeField] 
    private Outline outline;
    [SerializeField]
    private Button diceButton;

    public UnityEvent<UnitDice> OnEnterDice;
    public UnityEvent<UnitDice> OnExitDice;
    public UnityEvent<UnitDice> OnPressedDice;    

    private bool _isActive;
    private int _diceValue;
    private string _diceId;

    private void Start()
    {
        outline.enabled = false;
        diceButton.onClick.AddListener(() => OnPressedDice?.Invoke(this));

        _diceId = Random.Range(1000, 9999).ToString();
    }

    public void OnPointerEnter()
    {
        outline.enabled = true;

        OnEnterDice?.Invoke(this);
    }

    public void OnPointerExit()
    {
        outline.enabled = false;

        OnExitDice?.Invoke(this);
    }

    public void SetDiceValueToTentative()
    {
        diceText.text = "?";
    }

    public void SetDiceValue(int diceValue)
    {
        _diceValue = diceValue;
        diceText.text = $"{_diceValue}";
    }

    public void ActivateDice(bool condition)
    {
        _isActive = condition;
        gameObject.SetActive(condition);
    }
}
