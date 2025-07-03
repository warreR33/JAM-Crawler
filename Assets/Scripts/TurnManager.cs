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
        TurnFinished = false;
        runner.StartCoroutine(HandleTurn(character));
    }

    private static IEnumerator HandleTurn(Character character)
    {

        int index = RoundManager.Instance.turnOrder.IndexOf(character);
        UIManager.Instance.HighlightCurrentTurn(index);

        CurrentPhase = TurnPhase.TurnStart;
        yield return character.OnTurnStart();

        CurrentPhase = TurnPhase.TurnAction;
        yield return character.PerformAction();

        CurrentPhase = TurnPhase.TurnEnd;
        yield return character.OnTurnEnd();

        TurnFinished = true;
    }
}
