using UnityEngine;

public enum BuffType
{
    Regeneration,
    Strength,
    SpeedUp,
    Hardened,
}

public class Buff
{
    public BuffType Type { get; private set; }
    public int Duration { get; private set; }
    private int storedValue;
    private Character target;

    public Buff(BuffType type, int duration, Character target)
    {
        this.Type = type;
        this.Duration = duration;
        this.target = target;

        ApplyInitialEffect();
    }

    private void ApplyInitialEffect()
    {
        switch (Type)
        {
            case BuffType.Strength:
                storedValue = Mathf.CeilToInt(target.power * 0.3f);
                target.power += storedValue;
                Debug.Log($"{target.characterName} gana Strength: +{storedValue} poder.");
                break;

            case BuffType.SpeedUp:
                storedValue = Mathf.CeilToInt(target.speed * 0.3f);
                target.speed += storedValue;
                Debug.Log($"{target.characterName} gana SpeedUp: +{storedValue} velocidad.");
                break;

            case BuffType.Regeneration:
                // No hay efecto inicial, se aplica por turno
                break;
            case BuffType.Hardened:
                storedValue = Mathf.CeilToInt(target.defense * 0.3f);
                target.defense += storedValue;
                Debug.Log($"{target.characterName} gana Hardened: +{storedValue} defensa.");
                break;

        }
    }

    public void ApplyTurnEffect()
    {
        switch (Type)
        {
            case BuffType.Regeneration:
                int regenAmount = Mathf.CeilToInt(target.maxHP * 0.05f);
                target.Heal(regenAmount);
                Debug.Log($"{target.characterName} regenera {regenAmount} de HP por Regeneration.");
                break;
        }

        Duration--;
    }

    public void CleanUp()
    {
        switch (Type)
        {
            case BuffType.Strength:
                target.power -= storedValue;
                Debug.Log($"{target.characterName} pierde Strength: -{storedValue} poder.");
                break;

            case BuffType.SpeedUp:
                target.speed -= storedValue;
                Debug.Log($"{target.characterName} pierde SpeedUp: -{storedValue} velocidad.");
                break;

            case BuffType.Regeneration:
                // Nada que limpiar
                break;
        }
    }

    public bool IsExpired()
    {
        return Duration <= 0;
    }
}
