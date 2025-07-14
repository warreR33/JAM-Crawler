using UnityEngine;

public enum BuffType
{
    WeakRegeneration,
    StrongRegeneration,
    WeakStrength,
    StrongStrength,
    WeakSpeedUp,
    StrongSpeedUp,
    WeakHardened,
    StrongHardened
}

public enum BuffCategory
{
    Regeneration,
    Strength,
    SpeedUp,
    Hardened
}

public class Buff
{


    public BuffType Type { get; private set; }
    public int Duration { get; private set; }
    private int storedValue;
    private Character target;

    public BuffCategory Category
    {
        get
        {
            switch (Type)
            {
                case BuffType.WeakRegeneration:
                case BuffType.StrongRegeneration: return BuffCategory.Regeneration;

                case BuffType.WeakStrength:
                case BuffType.StrongStrength: return BuffCategory.Strength;

                case BuffType.WeakSpeedUp:
                case BuffType.StrongSpeedUp: return BuffCategory.SpeedUp;

                case BuffType.WeakHardened:
                case BuffType.StrongHardened: return BuffCategory.Hardened;

                default: throw new System.Exception("Unknown BuffType");
            }
        }
    }

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
            case BuffType.WeakStrength:
                storedValue = Mathf.CeilToInt(target.power * 0.25f);
                target.power += storedValue;
                Debug.Log($"{target.characterName} gana Weak Strength: +{storedValue} poder.");
                break;

            case BuffType.StrongStrength:
                storedValue = Mathf.CeilToInt(target.power * 0.5f);
                target.power += storedValue;
                Debug.Log($"{target.characterName} gana Strong Strength: +{storedValue} poder.");
                break;

            case BuffType.WeakSpeedUp:
                storedValue = Mathf.CeilToInt(target.speed * 0.15f);
                target.speed += storedValue;
                Debug.Log($"{target.characterName} gana Weak SpeedUp: +{storedValue} velocidad.");
                break;

            case BuffType.StrongSpeedUp:
                storedValue = Mathf.CeilToInt(target.speed * 0.3f);
                target.speed += storedValue;
                Debug.Log($"{target.characterName} gana Strong SpeedUp: +{storedValue} velocidad.");
                break;

            case BuffType.WeakHardened:
                storedValue = Mathf.CeilToInt(target.defense * 0.3f);
                target.defense += storedValue;
                Debug.Log($"{target.characterName} gana Weak Hardened: +{storedValue} defensa.");
                break;

            case BuffType.StrongHardened:
                storedValue = Mathf.CeilToInt(target.defense * 0.6f);
                target.defense += storedValue;
                Debug.Log($"{target.characterName} gana Strong Hardened: +{storedValue} defensa.");
                break;

            case BuffType.WeakRegeneration:
            case BuffType.StrongRegeneration:
                // Se aplica por turno
                break;
        }
    }

    public void ApplyTurnEffect()
    {
        switch (Type)
        {
            case BuffType.WeakRegeneration:
                int regenWeak = Mathf.CeilToInt(target.maxHP * 0.075f);
                target.Heal(regenWeak);
                Debug.Log($"{target.characterName} regenera {regenWeak} HP por Weak Regeneration.");
                break;

            case BuffType.StrongRegeneration:
                int regenStrong = Mathf.CeilToInt(target.maxHP * 0.15f);
                target.Heal(regenStrong);
                Debug.Log($"{target.characterName} regenera {regenStrong} HP por Strong Regeneration.");
                break;
        }

        Duration--;
    }

    public void CleanUp()
    {
        switch (Type)
        {
            case BuffType.WeakStrength:
            case BuffType.StrongStrength:
                target.power -= storedValue;
                Debug.Log($"{target.characterName} pierde Strength: -{storedValue} poder.");
                break;

            case BuffType.WeakSpeedUp:
            case BuffType.StrongSpeedUp:
                target.speed -= storedValue;
                Debug.Log($"{target.characterName} pierde SpeedUp: -{storedValue} velocidad.");
                break;

            case BuffType.WeakHardened:
            case BuffType.StrongHardened:
                target.defense -= storedValue;
                Debug.Log($"{target.characterName} pierde Hardened: -{storedValue} defensa.");
                break;

            case BuffType.WeakRegeneration:
            case BuffType.StrongRegeneration:
                // Nada que limpiar
                break;
        }
    }

    public bool IsExpired() => Duration <= 0;
}

