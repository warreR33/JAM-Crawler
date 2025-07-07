using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character
{
    [SerializeField] private AbilitySO defaultAbility;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

    }

    public override IEnumerator PerformAction()
    {
        if (defaultAbility == null)
        {
            Debug.LogWarning($"{characterName} no tiene habilidad asignada.");
            yield break;
        }

        Character target = SelectTarget();

        if (target == null)
        {
            Debug.LogWarning($"{characterName} no encontró objetivo válido.");
            yield break;
        }

        Debug.Log($"{characterName} va a usar {defaultAbility.abilityName} sobre {target.characterName}");

        yield return CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(defaultAbility.abilityName);
        yield return defaultAbility.ActivateRoutine(this, target);
        yield return CombatVisualFeedbackManager.Instance.EndAbilityFX();

        yield return new WaitForSeconds(0.5f);
    }

    private Character SelectTarget()
    {
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();

        List<PlayerCharacter> alivePlayers = new List<PlayerCharacter>();
        foreach (var p in players)
        {
            if (p.IsAlive)
                alivePlayers.Add(p);
        }

        if (alivePlayers.Count > 0)
        {
            return alivePlayers[UnityEngine.Random.Range(0, alivePlayers.Count)];
        }

        Debug.LogWarning($"{characterName} no tiene un objetivo válido.");
        return null;
    }
}
