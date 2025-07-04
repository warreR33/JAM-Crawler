using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    None,
    Fire,
    Water,
    Earth,
    Air
}

public enum StatType
{
    HP,
    Power,
    Defense,
    Speed,
    HPpercet
}


[CreateAssetMenu(menuName = "AbilitySystem/Actions/DamageAction")]
public class DamageActionSO : AbilityActionSO
{
    public StatType scalingStat;
    public float multiplier = 1f;
    public ElementType element = ElementType.None;

    public override void Execute(Character user, Character target, AbilitySO abilityUsed)
    {
        int baseValue = GetStatValue(user, scalingStat);
        int damage = Mathf.RoundToInt(baseValue * multiplier);
        target.TakeDamage(damage);

        Debug.Log($"{user.characterName} inflige {damage} de daño elemental {element} a {target.characterName}");

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
            //StatType.HPpercet 
            _ => 0
        };
    }
}
