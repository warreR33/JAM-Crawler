using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/DebuffAction")]
public class DebuffActionSO : AbilityActionSO
{
    public DebuffType debuffType;
    public int duration = 3;

    public override IEnumerator Execute(Character user, Character target, AbilitySO abilityUsed, SkillCheckResult result)
    {
        // Futuro: aumentar duración o aplicar debuff extra si Success
        target.AddDebuff(debuffType, duration);
        Debug.Log($"{target.characterName} recibe {debuffType} por {duration} turnos.");

        if (abilityUsed.impactEffectPrefab != null)
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);

        yield return null;
    }
}
