using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/DebuffAction")]
public class DebuffActionSO : AbilityActionSO
{
    public DebuffType weakDebuffType;    // Ej: burn_weak
    public DebuffType strongDebuffType;  // Ej: burn_strong
    public int weakDuration = 2;
    public int strongDuration = 4;

    public override IEnumerator Execute(Character user, Character target, AbilitySO abilityUsed, SkillCheckResult result)
    {
        if (target != null && target.IsAlive)
        {
            if (result == SkillCheckResult.Success)
            {
                target.AddDebuff(strongDebuffType, strongDuration);
            }
            else
            {
                target.AddDebuff(weakDebuffType, weakDuration);
            }
        }

        if (abilityUsed.impactEffectPrefab != null)
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);

        yield return null;
    }
}
