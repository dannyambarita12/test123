using CustomExtensions;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Card Card { get; private set; }

    [SerializeField]
    private Button cardButton;

    [Title("GENERAL INFORMATION")]
    [SerializeField]
    private TextMeshProUGUI cardNameText;
    [SerializeField]
    private Image cardDistanceIcon;
    [SerializeField]
    private Image cardIllustrationImage;

    [Title("EXPANDED INFORMATION")]
    [SerializeField]
    private CanvasGroup expandedInfoContainer;
    [SerializeField]
    private TokenValueItemUI[] tokenItems;

    [Title("MISC")]
    [SerializeField]
    private ShrinkAndExpand shrinkAndExpand;
    [SerializeField]
    private TokenImageDatabase tokenImageDatabase;

    public UnityEvent<CardUI> OnCardSelected = new();   

    private bool _isPeek;
    private bool _isSelected;
    private bool _isDisablePeek;
    private bool _currentVisualCondition;

    private void Awake()
    {
        cardButton.onClick.AddListener(() => OnCardSelected?.Invoke(this));
    }

    public void SetCard(Card card, UnityAction<CardUI> onSelectedCallback)
    {
        Card = card;
        UpdateCardVisual(card.CardData);

        if(onSelectedCallback != null)
            OnCardSelected.AddListener(onSelectedCallback);
    }

    public void HighlightCard(bool condition)
    {
        _isSelected = condition;
        ExpandCard(condition || _isPeek);
    }

    public void OnPointerEnter()
    {
        _isPeek = true;
        ExpandCard(_isPeek && !_isDisablePeek);
    }

    public void OnPointerExit()
    {
        _isPeek = false;
        ExpandCard(_isSelected);
    }

    public void DisablePeek(bool condition)
    {
        _isDisablePeek = condition;
    }

    private void ExpandCard(bool condition)
    {
        if(_currentVisualCondition == condition)
            return;

        _currentVisualCondition = condition;

        if (condition)
        {
            shrinkAndExpand.Expand(() => ShowExpandedInfo(true));
        }
        else
        {
            ShowExpandedInfo(false);
            shrinkAndExpand.Shrink();
        }
    }

    private void ShowExpandedInfo(bool condition)
    {
        float targetValue = condition ? 1f : 0f;
        //expandedInfoContainer.DOFade(targetValue, 0.1f);

        tokenItems.ForEach(x => x.ShowTokenValue(condition));
    }

    private void UpdateCardVisual(CardData cardData)
    {
        if (cardData == null)
            return;

        cardNameText.text = cardData.Name;
        cardDistanceIcon.sprite = tokenImageDatabase.GetCardDistanceIcon(cardData.DistanceType);

        var diceSequenceDatas = cardData.DiceDatas;
        var sprites = new Sprite[diceSequenceDatas.Count];
        tokenItems.ForEach(x => x.SetActive(false));
        for (int i = 0; i < diceSequenceDatas.Count; i++)
        {
            if(i < tokenItems.Length)
            {
                tokenItems[i].Setup(diceSequenceDatas[i]);
                tokenItems[i].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Token Item is not enough");
                break;
            }

            sprites[i] = tokenImageDatabase.GetTokenImage(diceSequenceDatas[i].Type).tokenSprite;
        }
    }
}
