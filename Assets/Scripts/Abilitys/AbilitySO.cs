using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    public string abilityName;

    [TextArea]
    public string description;
    public int energyCost;

    [Header("Targeting Rules")]
    public bool canTargetAllies = false;
    public bool canTargetEnemies = true;
    public bool isArea = false;

    public GameObject startEffectPrefab;   
    public GameObject impactEffectPrefab;


    public virtual IEnumerator ActivateRoutine(Character user, Character target)
    {
        yield break; 
    }
}
