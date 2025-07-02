using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public int energyCost;

    public GameObject visualEffectPrefab;


    public virtual void Activate(Character user, Character target)
    {
        Debug.Log($"{user.characterName} usa {abilityName} en {target.characterName}");
        // Lógica concreta en subclases
    }
}
