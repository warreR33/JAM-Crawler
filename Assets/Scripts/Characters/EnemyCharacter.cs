using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public override IEnumerator PerformAction()
    {
        Debug.Log($"{characterName} (enemigo) está actuando...");

        yield return new WaitForSeconds(1f);

        Debug.Log($"{characterName} (enemigo) termina su turno.");
    }
}
