using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public int energyCost;

    public GameObject startEffectPrefab;   
    public GameObject impactEffectPrefab;


    public virtual IEnumerator ActivateRoutine(Character user, Character target)
    {
        yield break; 
    }
}
