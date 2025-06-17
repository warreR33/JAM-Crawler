using UnityEngine;
using UnityEngine.UI;

public class UIActionPanel : MonoBehaviour
{
    public static UIActionPanel Instance;

    [Header("Turn UI")]
    public GameObject playerActionHUD;
    public Button endTurnButton;
    public Button actionButton1;
    public Button actionButton2;
    public Button actionButton3;
    public Button actionButton4;

    private System.Action onEndTurn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerActionHUD.SetActive(false);

        endTurnButton.onClick.AddListener(OnEndTurnPressed);
    }


    public void ShowPlayerActionHUD(PlayerCharacter character)
    {
        playerActionHUD.SetActive(true);
    }


    public void HidePlayerActionHUD()
    {
        playerActionHUD.SetActive(false);
    }

    public void SetEndTurnCallback(System.Action callback)
    {
        onEndTurn = callback;
    }

    public void ClearCallbacks()
    {
        onEndTurn = null;
    }

    private void OnEndTurnPressed()
    {
        onEndTurn?.Invoke();
    }


    public void SetCurrentTurn(Character current)
    {
        if (current.team == TeamType.Player)
        {
            Debug.Log("Turno del jugador");
        }
        else
        {
            Debug.Log("Turno del enemigo");
        }
    }

    public void SetUpcomingTurn(Character next)
    {
        Debug.Log($"Siguiente turno: {next.characterName}");
    }
}
