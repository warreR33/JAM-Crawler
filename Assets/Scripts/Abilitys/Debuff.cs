using UnityEngine;

public enum DebuffType
{
    Burn,
    Dizzy
}

public class Debuff
{
    public DebuffType Type { get; private set; }
    public int Duration { get; private set; }
    private int storedValue; 
    private Character target;

    public Debuff(DebuffType type, int duration, Character target)
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
            case DebuffType.Dizzy:
                storedValue = Mathf.CeilToInt(target.speed * 0.1f);
                target.speed -= storedValue;
                Debug.Log($"{target.characterName} sufre Dizzy: velocidad reducida en {storedValue}");
                break;

            case DebuffType.Burn:
                // Burn no tiene efecto inicial
                break;
        }
    }

    public void ApplyTurnEffect()
    {
        switch (Type)
        {
            case DebuffType.Burn:
                int burnDamage = Mathf.CeilToInt(target.maxHP * 0.05f);
                target.TakeDamage(burnDamage);
                Debug.Log($"{target.characterName} sufre {burnDamage} de daño por Burn.");
                break;

            case DebuffType.Dizzy:
                // Nada por turno, el efecto es inmediato
                break;
        }

        Duration--;
    }

    public void CleanUp()
    {
        switch (Type)
        {
            case DebuffType.Dizzy:
                target.speed += storedValue;
                Debug.Log($"{target.characterName} recupera {storedValue} de velocidad al terminar Dizzy.");
                break;

            case DebuffType.Burn:
                // Nada que limpiar
                break;
        }
    }

    public bool IsExpired()
    {
        return Duration <= 0;
    }
}
