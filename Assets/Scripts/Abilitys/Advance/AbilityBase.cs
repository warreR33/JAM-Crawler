using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Ability Base")]
public class AbilityBase : AbilitySO
{
    public List<AbilityActionSO> actions;

    public override IEnumerator ActivateRoutine(Character user, Character target)
    {
        // Habilidad normal (no en área)
        if (!isArea)
        {
            yield return ExecuteSingle(user, target);
        }
        else
        {
            // Obtener todos los objetivos válidos según reglas
            List<Character> targets = new List<Character>();

            if (canTargetEnemies)
                targets.AddRange(FindCharactersByTeam(TeamType.Enemy));

            if (canTargetAllies)
                targets.AddRange(FindCharactersByTeam(TeamType.Player));

            // Mostrar efecto y animación SOLO una vez
            yield return CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(abilityName);

            if (startEffectPrefab != null)
            {
                GameObject.Instantiate(startEffectPrefab, user.SpriteWorldPosition, Quaternion.identity);
                yield return new WaitForSeconds(0.3f);
            }

            foreach (var t in targets)
            {
                foreach (var action in actions)
                {
                    action.Execute(user, t, this);

                    if (impactEffectPrefab != null)
                        GameObject.Instantiate(impactEffectPrefab, t.transform.position, Quaternion.identity);
                }

                yield return new WaitForSeconds(0.1f); // Pequeña pausa entre cada objetivo
            }

            yield return CombatVisualFeedbackManager.Instance.EndAbilityFX();
        }
    }

    private IEnumerator ExecuteSingle(Character user, Character target)
    {
        yield return CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(abilityName);

        if (startEffectPrefab != null)
        {
            GameObject.Instantiate(startEffectPrefab, user.SpriteWorldPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        foreach (var action in actions)
        {
            action.Execute(user, target, this);

            if (impactEffectPrefab != null)
            {
                GameObject.Instantiate(impactEffectPrefab, target.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return CombatVisualFeedbackManager.Instance.EndAbilityFX();
    }

    private List<Character> FindCharactersByTeam(TeamType team)
    {
        List<Character> result = new List<Character>();
        foreach (Character c in GameObject.FindObjectsOfType<Character>())
        {
            if (c.team == team && c.IsAlive)
                result.Add(c);
        }
        return result;
    }
}
