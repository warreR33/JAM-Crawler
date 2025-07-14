using UnityEngine;
public enum DebuffType
{
    WeakBurn,
    StrongBurn,
    WeakDizzy,
    StrongDizzy,
    WeakWeaken,
    StrongWeaken,
    WeakBleeding,
    StrongBleeding
}

public enum DebuffCategory
{
    Burn,
    Dizzy,
    Weaken,
    Bleeding
}


public class Debuff
{

   

    public DebuffType Type { get; private set; }
    public int Duration { get; private set; }
    private int storedValue;
    private Character target;
    
    public DebuffCategory Category
    {
        get
        {
            switch (Type)
            {
                case DebuffType.WeakBurn:
                case DebuffType.StrongBurn: return DebuffCategory.Burn;

                case DebuffType.WeakDizzy:
                case DebuffType.StrongDizzy: return DebuffCategory.Dizzy;

                case DebuffType.WeakWeaken:
                case DebuffType.StrongWeaken: return DebuffCategory.Weaken;

                case DebuffType.WeakBleeding:
                case DebuffType.StrongBleeding: return DebuffCategory.Bleeding;

                default: throw new System.Exception("Unknown DebuffType");
            }
        }
    }

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
            case DebuffType.WeakDizzy:
                storedValue = Mathf.CeilToInt(target.speed * 0.15f);
                target.speed -= storedValue;
                Debug.Log($"{target.characterName} sufre Weak Dizzy: velocidad reducida en {storedValue}");
                break;

            case DebuffType.StrongDizzy:
                storedValue = Mathf.CeilToInt(target.speed * 0.30f);
                target.speed -= storedValue;
                Debug.Log($"{target.characterName} sufre Strong Dizzy: velocidad reducida en {storedValue}");
                break;

            case DebuffType.WeakWeaken:
                storedValue = Mathf.CeilToInt(target.defense * 0.30f);
                target.defense -= storedValue;
                Debug.Log($"{target.characterName} sufre Weak Weaken: defensa reducida en {storedValue}");
                break;

            case DebuffType.StrongWeaken:
                storedValue = Mathf.CeilToInt(target.defense * 0.50f);
                target.defense -= storedValue;
                Debug.Log($"{target.characterName} sufre Strong Weaken: defensa reducida en {storedValue}");
                break;

            case DebuffType.WeakBurn:
            case DebuffType.StrongBurn:
            case DebuffType.WeakBleeding:
            case DebuffType.StrongBleeding:
                // Sin efecto inmediato
                break;
        }
    }

    public void ApplyTurnEffect()
    {
        switch (Type)
        {
            case DebuffType.WeakBurn:
                int burn1 = Mathf.CeilToInt(target.maxHP * 0.025f);
                target.TakeDamage(burn1);
                Debug.Log($"{target.characterName} sufre {burn1} de daño por Weak Burn.");
                break;

            case DebuffType.StrongBurn:
                int burn2 = Mathf.CeilToInt(target.maxHP * 0.05f);
                target.TakeDamage(burn2);
                Debug.Log($"{target.characterName} sufre {burn2} de daño por Strong Burn.");
                break;

            case DebuffType.WeakBleeding:
            case DebuffType.StrongBleeding:
                // Se manejará en el sistema de curación
                break;
        }

        Duration--;
    }

    public void CleanUp()
    {
        switch (Type)
        {
            case DebuffType.WeakDizzy:
            case DebuffType.StrongDizzy:
                target.speed += storedValue;
                Debug.Log($"{target.characterName} recupera {storedValue} de velocidad.");
                break;

            case DebuffType.WeakWeaken:
            case DebuffType.StrongWeaken:
                target.defense += storedValue;
                Debug.Log($"{target.characterName} recupera {storedValue} de defensa.");
                break;
        }
    }

    public bool IsExpired() => Duration <= 0;

    public float GetHealingModifier()
    {
        switch (Type)
        {
            case DebuffType.WeakBleeding: return 0.5f;
            case DebuffType.StrongBleeding: return 0.0f; // 100% reducción
            default: return 1f; // no afecta curación
        }
    }
}

