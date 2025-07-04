using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Ability Base")]
public class AbilityBase : AbilitySO
{
    public List<AbilityActionSO> actions;

    public override IEnumerator ActivateRoutine(Character user, Character target)
    {
        yield return CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(abilityName);

        if (startEffectPrefab != null)
        {
            GameObject.Instantiate(startEffectPrefab, user.transform.position, Quaternion.identity);
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
}
