using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    RoundStart,
    TurnStart,
    TurnAction,
    TurnEnd,
    RoundEnd
}

public class TurnManager : MonoBehaviour
{
    public static bool TurnFinished { get; private set; }

    public enum TurnPhase { TurnStart, TurnAction, TurnEnd }
    public static TurnPhase CurrentPhase { get; private set; }

    private static MonoBehaviour runner;

    private void Awake()
    {
        runner = this;
    }

    public static void StartTurn(Character character)
    {
        if (CheckBattleEnd()) return;
        
        TurnFinished = false;
        runner.StartCoroutine(HandleTurn(character));
    }

    private static IEnumerator HandleTurn(Character character)
    {

        int index = RoundManager.Instance.turnOrder.IndexOf(character);
        if (!TurnUIManager.Instance.IsAnimatingReset)
        {
            TurnUIManager.Instance.HighlightCurrentTurn(index);
        }

        CurrentPhase = TurnPhase.TurnStart;
        yield return character.OnTurnStart();

        CurrentPhase = TurnPhase.TurnAction;
        yield return character.PerformAction();

        CurrentPhase = TurnPhase.TurnEnd;
        yield return character.OnTurnEnd();

        TurnFinished = true;
    }
    

    private static bool CheckBattleEnd()
    {
        Character[] characters = GameObject.FindObjectsOfType<Character>();

        bool enemiesAlive = false;
        bool playersAlive = false;

        foreach (var c in characters)
        {
            if (!c.IsAlive) continue;

            if (c.team == TeamType.Player) playersAlive = true;
            if (c.team == TeamType.Enemy) enemiesAlive = true;
        }

        if (!playersAlive)
        {
            Debug.Log("¡Has sido derrotado!");
            // Ir a pantalla de derrota
            return true;
        }

        if (!enemiesAlive)
        {
            Debug.Log("¡Ganaste el combate!");
            GameStateManager.Instance.LoadDungeonScene();
            return true;
        }

        return false; // La batalla sigue
    }
}
