using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public override IEnumerator PerformAction()
    {
        // Por ahora el enemigo simplemente espera un segundo y termina su turno.
        Debug.Log($"{characterName} (enemigo) está actuando...");

        // Acá más adelante irán las acciones del enemigo (moverse, atacar, etc.)
        yield return new WaitForSeconds(1f);

        Debug.Log($"{characterName} (enemigo) termina su turno.");
    }
}
