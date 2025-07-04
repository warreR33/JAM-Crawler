using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AbilitySystem/Actions/DebuffAction")]
public class DebuffActionSO : AbilityActionSO
{
    public DebuffType debuffType;
    public int duration = 3;

    public override void Execute(Character user, Character target, AbilitySO abilityUsed)
    {
        target.AddDebuff(debuffType, duration);
        Debug.Log($"{target.characterName} recibe {debuffType} por {duration} turnos.");

        if (abilityUsed.impactEffectPrefab != null)
        {
            GameObject.Instantiate(abilityUsed.impactEffectPrefab, target.transform.position, Quaternion.identity);
        }
    }
}
