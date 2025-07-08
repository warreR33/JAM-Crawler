using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/HealAction")]
public class HealActionSO : AbilityActionSO
{
    public StatType scalingStat;
    public float multiplier = 1f;
    public ElementType element = ElementType.None; 

    public override void Execute(Character user, Character target, AbilitySO abilityUsed)
    {
        int baseValue = GetStatValue(user, scalingStat);
        int healAmount = Mathf.RoundToInt(baseValue * multiplier);

        target.Heal(healAmount);

        Debug.Log($"{user.characterName} cura {healAmount} de vida elemental {element} a {target.characterName}");

        if (abilityUsed.impactEffectPrefab != null)
        {
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);
        }
    }

    private int GetStatValue(Character character, StatType stat)
    {
        return stat switch
        {
            StatType.HP => character.currentHP,
            StatType.Power => character.power,
            StatType.Defense => character.defense,
            StatType.Speed => character.speed,
            // Podés agregar más casos si quieres
            _ => 0
        };
    }
}
