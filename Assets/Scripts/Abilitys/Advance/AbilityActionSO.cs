using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityActionSO : ScriptableObject
{
    public abstract void Execute(Character user, Character target);
}
