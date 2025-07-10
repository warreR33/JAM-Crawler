using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/Buff")]
public class BuffActionSO : AbilityActionSO
{
    public BuffType buffType;
    public int duration = 3;

    public override IEnumerator Execute(Character user, Character target, AbilitySO abilityUsed, SkillCheckResult result)
    {
        if (target != null && target.IsAlive)
        {
            // Futuro: mejorar duración o fuerza del buff si result == Success
            target.AddBuff(buffType, duration);
            Debug.Log($"{target.characterName} recibe el buff {buffType} por {duration} turnos.");
        }

        yield return null;
    }
}
