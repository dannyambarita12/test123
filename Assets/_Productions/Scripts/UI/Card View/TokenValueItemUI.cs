using CustomExtensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TokenValueItemUI : MonoBehaviour
{
    [SerializeField]
    private Image tokenIcon;
    [SerializeField]
    private TextMeshProUGUI tokenValueText;

    [SerializeField]
    private TokenImageDatabase tokenImageDatabase;

    public void Setup(CardToken token)
    {
        var tokenAssetData = tokenImageDatabase.GetTokenImage(token.Type);
        tokenIcon.sprite = tokenAssetData.tokenSprite;
        tokenValueText.color = tokenAssetData.tokenColor;
        tokenValueText.text = $"{token.MinValue} - {token.MaxValue}";
    }

    [Button]
    public void ShowTokenValue(bool condition)
    {
        tokenValueText.SetActive(condition);
    }
}
