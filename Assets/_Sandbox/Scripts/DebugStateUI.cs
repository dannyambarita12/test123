using TMPro;
using UnityEngine;

public class DebugStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ShowState), 0.5f, 1);
    }

    private void ShowState()
    {
        stateText.text = gameStateManager ? gameStateManager.CurrentState.ToString() : "No Game State Manager";
    }
}
