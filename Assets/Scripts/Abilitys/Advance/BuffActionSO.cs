using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/Actions/Buff")]
public class BuffActionSO : AbilityActionSO
{
    public BuffType buffType;
    public int duration = 3;

    public override void Execute(Character user, Character target, AbilitySO abilityUsed)
    {
        if (target != null && target.IsAlive)
        {
            target.AddBuff(buffType, duration);
            Debug.Log($"{target.characterName} recibe el buff {buffType} por {duration} turnos.");
        }
    }
}
