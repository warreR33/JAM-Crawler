using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public List<Character> turnOrder = new();
    public int roundCount = 1;

    public static int CurrentRound => Instance != null ? Instance.roundCount : 1;

    private int currentCharacterIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartRoundLoop());
    }

    private IEnumerator StartRoundLoop()
    {
        while (true)
        {
            Debug.Log($"--- Comienza la Ronda {roundCount} ---");

            yield return StartCoroutine(OnRoundStart());

            currentCharacterIndex = 0;

            while (currentCharacterIndex < turnOrder.Count)
            {
                Character currentCharacter = turnOrder[currentCharacterIndex];
                TurnManager.StartTurn(currentCharacter);
                yield return new WaitUntil(() => TurnManager.TurnFinished);

                currentCharacterIndex++;
            }

            yield return StartCoroutine(OnRoundEnd());

            roundCount++;
        }
    }

    private IEnumerator OnRoundStart()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();
        turnOrder = new List<Character>(allCharacters);

            
        foreach (var character in turnOrder)
        {
            character.initiativeRoll = Random.Range(1, 11); // 1 a 10
        }

        turnOrder.Sort((a, b) =>
        {
            int aTotal = a.speed + a.initiativeRoll;
            int bTotal = b.speed + b.initiativeRoll;
            return bTotal.CompareTo(aTotal);
        });

        currentCharacterIndex = 0;

        UIManager.Instance.UpdateTurnOrder(turnOrder);

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator OnRoundEnd()
    {
        Debug.Log("Fin de la Ronda");
        yield return new WaitForSeconds(0.5f);
    }
}
