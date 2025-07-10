using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/HealAction")]
public class HealActionSO : AbilityActionSO
{
    public StatType scalingStat;
    public float multiplier = 1f;
    public ElementType element = ElementType.None;

    public override IEnumerator Execute(Character user, Character target, AbilitySO abilityUsed, SkillCheckResult result)
    {
        int baseValue = GetStatValue(user, scalingStat);
        int healAmount = Mathf.RoundToInt(baseValue * multiplier);

        // Futuro: aumentar curación si result == SkillCheckResult.Success
        target.Heal(healAmount);

        Debug.Log($"{user.characterName} cura {healAmount} de vida elemental {element} a {target.characterName}");

        if (abilityUsed.impactEffectPrefab != null)
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);

        yield return null;
    }

    private int GetStatValue(Character character, StatType stat)
    {
        return stat switch
        {
            StatType.HP => character.currentHP,
            StatType.Power => character.power,
            StatType.Defense => character.defense,
            StatType.Speed => character.speed,
            _ => 0
        };
    }
}
