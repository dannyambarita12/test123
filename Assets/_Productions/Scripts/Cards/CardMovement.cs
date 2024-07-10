using UnityEngine;
using System.Threading.Tasks;

public class CardMovement : MonoBehaviour
{
    public PointerState pointerState;
    public enum PointerState { PointerEnter, PointerExit, }

    public CardState cardState;
    public enum CardState { FollowMouse, Aim, Standby, FlyingToStandby, }

    private int cardIndex;
    private int cardInHandCount;
    private bool isTaskDelayRunning;
    private bool isSelected;
    private Vector3 initialPosition;
    private Vector3 cardPosition;

    [SerializeField] private CardMovement cardMovement;

    [Header("Card Setting")]
    [SerializeField] private float cardTransformSpeed;
    [SerializeField] private int cardFlyingTransformSpeed;
    [SerializeField] private int cardFlyingDuration;
    [SerializeField] private float cardYPosition;
    [SerializeField] private float cardWidth;
    [SerializeField] private float spacing;
    [SerializeField] private float ScaleBigVerticalOffset;
    [SerializeField] private Vector3 cardScaleBig;
    [SerializeField] private Vector3 cardOnAimPosition;

    private void Awake()
    {
        transform.position = new Vector3(0,0,0);

        isTaskDelayRunning = false;
        pointerState = PointerState.PointerExit;
        cardState = CardState.FlyingToStandby;
    }

    public void SetCardIndex(int index)
    {
        cardIndex = index;

        cardPosition = GetCardPosition();
    }
    public int GetCardIndex()
    {
        return cardIndex;
    }

    public void SetCardInHandCount(int amount)
    {
        cardInHandCount += amount;

        cardPosition = GetCardPosition();
    }

    public void ButtonOnClick()
    {
        ButtonOnClickOnCombat();
    }

    public void ButtonOnClickOnCombat()
    {
        switch (cardState)
        {
            case CardState.Standby:
                cardState = CardState.FollowMouse;
                // BattleManager.instance.SetSelectedCardUI(this);
                // BattleManager.instance.EnableAllEnemyButton();
                break;
            case CardState.FollowMouse:
                cardState = CardState.FlyingToStandby;
                // BattleManager.instance.ClearSelectedCardUI();
                // BattleManager.instance.DisableAllEnemyButton();
                break;
            default:
                Debug.Log("wtf");
                break;
        }
    }

    public void OnPointerEnter()
    {
        pointerState = PointerState.PointerEnter;
    }

    public void OnPointerExit()
    {
        pointerState = PointerState.PointerExit;
    }

    // public async Task Update()
    // {
    //     if (cardPosition != Vector3.zero)
    //     {
    //         await UpdateCardPosition(cardPosition);
    //     }
    // }

    private async Task UpdateCardPosition(Vector3 cardPositionAtIndex)
    {
        switch (cardState)
        {
            case CardState.FollowMouse:

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    cardState = CardState.Aim;
                    break;
                }

                transform.position = Vector3.Lerp(transform.position, Input.mousePosition, cardTransformSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, cardScaleBig, cardTransformSpeed * Time.deltaTime);
                break;

            case CardState.Aim:

                if (Input.mousePosition.y < 50 || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    cardState = CardState.FlyingToStandby;
                    // BattleManager.instance.ClearSelectedCardUI();
                    // BattleManager.instance.DisableAllEnemyButton();
                    break;
                }

                transform.localPosition = Vector3.Lerp(transform.localPosition, cardOnAimPosition, cardTransformSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, cardScaleBig, cardTransformSpeed * Time.deltaTime);
                break;

            case CardState.Standby:

                if (pointerState == PointerState.PointerEnter)
                {
                    transform.localPosition = cardPositionAtIndex + Vector3.up * ScaleBigVerticalOffset;
                    transform.localScale = cardScaleBig;
                    break;
                }

                transform.localPosition = Vector3.Lerp(transform.localPosition, cardPositionAtIndex, cardTransformSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, cardTransformSpeed * Time.deltaTime);
                break;

            case CardState.FlyingToStandby:

                transform.localPosition = Vector3.Lerp(transform.localPosition, cardPositionAtIndex, cardFlyingTransformSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, cardFlyingTransformSpeed * Time.deltaTime);
                
                if (!isTaskDelayRunning)
                {
                    isTaskDelayRunning = true;
                    await Task.Delay(cardFlyingDuration);
                    cardState = CardState.Standby;
                    isTaskDelayRunning = false;
                }
                break;

            default:
                break;
        }
    }

    private Vector3 GetCardPosition()
    {
        float totalCardWidth = cardWidth + spacing;

        float handWidth = (cardInHandCount - 1) * totalCardWidth;

        float initialXPosition = -handWidth / 2f;

        float cardXPosition = initialXPosition + (cardIndex * totalCardWidth);

        return new Vector3(cardXPosition, cardYPosition, 0);
    }
}
