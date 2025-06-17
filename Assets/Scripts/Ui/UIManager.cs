using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Turn Order UI")]
    public TMP_Text turnOrderText;      
    public TMP_Text currentTurnText;   

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTurnOrder(List<Character> characters)
    {
        string result = "Turnos:\n";

        for (int i = 0; i < characters.Count; i++)
        {
            Character c = characters[i];
            int total = c.speed + c.initiativeRoll;
            result += $"{i + 1}. {c.characterName} (VEL: {total} = {c.speed} + {c.initiativeRoll})\n";
        }

        turnOrderText.text = result.TrimEnd(); 
    }

    public void SetCurrentTurn(Character character)
    {
        currentTurnText.text = $"Turno actual: {character.characterName} \n Ronda:{RoundManager.CurrentRound}";
    }
}
