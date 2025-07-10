using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Ability Base")]
public class AbilityBase : AbilitySO
{
    public List<AbilityActionSO> actions;

    [SerializeField] private GameObject quickTimeEventPrefab;

    public override IEnumerator ActivateRoutine(Character user, Character target)
    {
        // Habilidad normal (no en área)
        if (!isArea)
        {
            yield return ExecuteSingle(user, target);
        }
        else
        {
            // Área (opcional de actualizar después)
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
            SkillCheckResult result = SkillCheckResult.Normal;
            bool isDone = false;

            GameObject qteGO = GameObject.Instantiate(quickTimeEventPrefab, GameObject.Find("Canvas").transform);
            SkillCheckUI qte = qteGO.GetComponent<SkillCheckUI>();

            qte.StartSkillCheck((r) =>
            {
                result = r;
                isDone = true;
            });

            yield return new WaitUntil(() => isDone);
            yield return action.Execute(user, target, this, result);

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
