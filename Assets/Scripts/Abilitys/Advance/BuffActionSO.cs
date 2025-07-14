using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/Buff")]
public class BuffActionSO : AbilityActionSO
{
    public BuffType weakBuffType;
    public BuffType strongBuffType;
    public int weakDuration = 2;
    public int strongDuration = 4;

    public override IEnumerator Execute(Character user, Character target, AbilitySO abilityUsed, SkillCheckResult result)
    {
        if (target != null && target.IsAlive)
        {
            if (result == SkillCheckResult.Success)
            {
                target.AddBuff(strongBuffType, strongDuration);
            }
            else
            {
                target.AddBuff(weakBuffType, weakDuration);
            }
        }

        // Si querés, podés agregar efectos visuales igual que en DebuffActionSO
        if (abilityUsed.impactEffectPrefab != null)
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);

        yield return null;
    }
}
