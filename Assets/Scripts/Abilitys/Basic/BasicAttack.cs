using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Basic Attack")]

public class BasicAttack : AbilitySO
{
    public float powerMultiplier = 1.2f;

    public override void Activate(Character user, Character target)
    {
        if (!user.IsAlive || !target.IsAlive) return;

        float attackPower = user.power * powerMultiplier;
        float damage = Mathf.Max(0, attackPower - target.defense);

        target.TakeDamage(Mathf.RoundToInt(damage));
        Debug.Log($"{user.characterName} usa {abilityName} en {target.characterName}, causando {damage} de daño.");
    }
}
