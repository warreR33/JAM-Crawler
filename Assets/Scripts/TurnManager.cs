using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public enum Phase
    {
        RoundStart,
        TurnStart,
        TurnAction,
        TurnEnd,
        RoundEnd
    }

    public List<Character> turnOrder = new List<Character>(); 
    private int currentCharacterIndex = 0;
    private int roundCount = 1;
    private bool roundInProgress = false;

    public Phase currentPhase;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(RoundLoop());
    }

    private IEnumerator RoundLoop()
    {
        while (true) 
        {
            //Debug.Log($"--- Comienza la Ronda {roundCount} ---");
            currentPhase = Phase.RoundStart;
            yield return StartCoroutine(OnRoundStart());

            currentPhase = Phase.TurnStart;
            roundInProgress = true;
            currentCharacterIndex = 0;

            while (roundInProgress && currentCharacterIndex < turnOrder.Count)
            {
                Character current = turnOrder[currentCharacterIndex];
                yield return StartCoroutine(HandleTurn(current));
                currentCharacterIndex++;
            }

            currentPhase = Phase.RoundEnd;
            yield return StartCoroutine(OnRoundEnd());

            roundCount++;
        }
    }

    private IEnumerator OnRoundStart()
    {
        //Debug.Log("Inicio de la Ronda");

        // Detectar todos los personajes activos y ordenarlos por velocidad descendente
        turnOrder = new List<Character>(FindObjectsOfType<Character>());
        turnOrder.Sort((a, b) => b.speed.CompareTo(a.speed));

        currentCharacterIndex = 0;

        // Actualizar visual de la lista de turnos
        UIManager.Instance.UpdateTurnOrder(turnOrder);

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator HandleTurn(Character character)
    {
        UIManager.Instance.SetCurrentTurn(character);

        currentPhase = Phase.TurnStart;
        yield return StartCoroutine(character.OnTurnStart());

        currentPhase = Phase.TurnAction;
        yield return StartCoroutine(character.PerformAction());

        currentPhase = Phase.TurnEnd;
        yield return StartCoroutine(character.OnTurnEnd());

        // Previsualizar próximo personaje (si no terminó la ronda)
        if (currentCharacterIndex + 1 < turnOrder.Count)
        {
            Character nextCharacter = turnOrder[currentCharacterIndex + 1];
            UIActionPanel.Instance.SetUpcomingTurn(nextCharacter);
        }
    }

    private IEnumerator OnRoundEnd()
    {
        //Debug.Log("Fin de la Ronda");
        yield return new WaitForSeconds(0.5f);
    }
}
