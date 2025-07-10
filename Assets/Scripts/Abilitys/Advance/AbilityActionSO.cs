using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityActionSO : ScriptableObject
{
    public abstract IEnumerator  Execute(Character user, Character target, AbilitySO abilityUsed,SkillCheckResult result);
}
