using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailContainer : MonoBehaviour
{
    [Title("Unit Detail Components")]
    [SerializeField]
    private Image portraitImage;
    [SerializeField]
    private TextMeshProUGUI nameText;    

    [Title("Card Detail Components")]
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private CardUI cardItemUI;

    [Title("General Components")]
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Unit _currentSelectedUnit;
    private Unit _currentPeekUnit;
    private bool _isPeek;
    private bool _isSelected;

    private Card _currentSelectedCard;
    private Card _currentPeekCard;
    private bool _isCardPeek;
    private bool _isCardSelected;

    private void Start()
    {
        container.SetActive(false);
    }

    public void Select(bool condition)
    {
        _isSelected = condition;

        if(_currentPeekUnit != null)
            _currentSelectedUnit = _currentPeekUnit;

        ActiveContainer(_isPeek);
    }

    public void Peek(Unit unit)
    {
        _isPeek = true;
        _currentPeekUnit = unit;
        
        ActiveContainer(_isPeek);
        ShowUnitDetail(_currentPeekUnit);
    }

    public void ClosePeek()
    {
        _isPeek = false;
           
        bool isOpen = _isPeek || _isSelected;
        ActiveContainer(isOpen);

        if (isOpen && _currentSelectedUnit != null)
        {
            ShowUnitDetail(_currentSelectedUnit);
        }
        else
        {
            _currentPeekUnit = null;
        }
    }
    
    public void SelectCard(bool condition)
    {
        _isCardSelected = condition;

        if (_isCardSelected)
        {
            if (_currentPeekCard != null)
                _currentSelectedCard = _currentPeekCard;

            var hasCard = _currentSelectedCard != null;
            if (hasCard)
                ShowCardDetail(_currentSelectedCard);

            ShowCard(hasCard);
        }
        else
        {
            _currentSelectedCard = null;
        }
    }

    public void PeekCard(Card card)
    {
        _isCardPeek = true;
        _currentPeekCard = card;

        ShowCard(_isCardPeek && card != null);
        
        if (_currentPeekCard != null)
        {
            ShowCardDetail(_currentSelectedCard);
        }
    }

    public void ClosePeekCard()
    {
        _isCardPeek = false;

        bool isOpen = _isCardPeek || _isCardSelected;

        if (isOpen && _currentSelectedCard != null)
        {
            ShowCard(isOpen);
            ShowCardDetail(_currentSelectedCard);
        }
        else
        {
            _currentPeekCard = null;
        }
    }

    private void ActiveContainer(bool condition)
    {
        canvasGroup.alpha = condition ? 1 : 0;
        canvasGroup.interactable = condition;
        canvasGroup.blocksRaycasts = condition;
    }

    private void ShowUnitDetail(Unit unit)
    {
        portraitImage.sprite = unit.UnitData.Portrait;
        nameText.text = unit.UnitData.Name;
    }

    private void ShowCardDetail(Card card)
    {
        //cardItemUI.SetCard(card, null);
    }

    private void ShowCard(bool condition)
    {
        container.SetActive(condition);
    }
}
