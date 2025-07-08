using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Basic Attack")]
public class BasicAttack : AbilitySO
{
    public float powerMultiplier = 1.2f;

    public override IEnumerator ActivateRoutine(Character user, Character target)
    {
        if (!user.IsAlive || !target.IsAlive)
        {
            yield break;
        }

        yield return CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(abilityName);

        if (startEffectPrefab != null)
        {
            GameObject.Instantiate(startEffectPrefab, user.SpriteWorldPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        float attackPower = user.power * powerMultiplier;
        float damage = Mathf.Max(0, attackPower - target.defense);

        target.TakeDamage(Mathf.RoundToInt(damage));
        Debug.Log($"{user.characterName} usa {abilityName} en {target.characterName}, causando {damage} de daño.");

        if (impactEffectPrefab != null)
        {
            GameObject.Instantiate(impactEffectPrefab, target.transform.position, Quaternion.identity);
        }

        if (user is PlayerCharacter player)
        {
            player.GainEnergy(1);
        }

        yield return new WaitForSeconds(0.2f);
        yield return CombatVisualFeedbackManager.Instance.EndAbilityFX();
    }
}
