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

    // Callback para finalizar turno
    private System.Action onEndTurn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerActionHUD.SetActive(false);

        endTurnButton.onClick.AddListener(OnEndTurnPressed);

        // Podés enlazar los otros botones si querés acciones específicas más adelante
        // actionButton1.onClick.AddListener(() => HacerAlgo());
    }


    public void ShowPlayerActionHUD(PlayerCharacter character)
    {
        playerActionHUD.SetActive(true);

        // Aquí podrías habilitar/deshabilitar botones según AP, estado, etc.
        // Por ahora asumimos que todos los botones están disponibles
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
        onEndTurn?.Invoke(); // Ejecuta el callback si está asignado
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
        // Podés actualizar un texto o icono en la interfaz si querés
    }
}
