using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Ability Base")]
public class AbilityBase : AbilitySO
{
    public List<AbilityActionSO> actions;

        public override void Activate(Character user, Character target)
        {
            Debug.Log($"{user.characterName} usa {abilityName} en {target.characterName}");

            foreach (var action in actions)
            {
                action.Execute(user, target);
            }
        }
}
