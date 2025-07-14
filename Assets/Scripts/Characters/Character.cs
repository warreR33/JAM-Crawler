using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Player,
    Enemy
}

public abstract class Character : MonoBehaviour, ISelectable
{

    private StatusEffectIconDatabase iconDatabase;
    public System.Action OnPlayerActionCompleted;

    [Header("Identidad")]
    public string characterName;
    public TeamType team;

    [Header("Turnos")]
    public int maxEnergy = 10;
    public int currentEnergy = 0;
    public int speed = 10;
    [HideInInspector] public int initiativeRoll = 0;

    [Header("Vida")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Combate")]
    public int power = 10;                 
    public int defense = 5;               
      
    public float critMultiplier = 1.5f;   
    

    public bool IsAlive => currentHP > 0;
    private Vector3? originalLocalPosition = null;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSelected = false;
    private bool isHovered = false;

    private List<Debuff> activeDebuffs = new List<Debuff>();
    private List<Buff> activeBuffs = new List<Buff>();

    public CharacterHUD HUD { get; set; }

    public Vector3 SpriteWorldPosition => spriteRenderer.transform.position;
    
    public Sprite icon; 

    public Animator Animator { get; private set; }



    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        Animator = GetComponentInChildren<Animator>();
        
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);

        StartCoroutine(ShakeSprite());
    }
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public void AddDebuff(DebuffType type, int duration)
    {
        var newDebuff = new Debuff(type, duration, this);
        var category = DebuffUtils.GetCategory(type);

        Debuff existing = activeDebuffs.Find(d => d.Category == category);
        if (existing != null)
        {
            existing.CleanUp();
            activeDebuffs.Remove(existing);
            HUD?.RemoveDebuffIcon(existing.Type); // ← ahora se usa DebuffType
        }

        activeDebuffs.Add(newDebuff);

        Sprite icon = StatusEffectIconDatabase.GetDebuffIcon(type); // ← estático
        HUD?.AddDebuffIcon(type, icon, duration);
    }

    public void AddBuff(BuffType type, int duration)
    {
        var newBuff = new Buff(type, duration, this);
        var category = newBuff.Category;

        Debug.Log($"[Character] AddBuff llamado con {type}, duración {duration}");

        // Reemplazar cualquier buff existente de la misma categoría
        Buff existing = activeBuffs.Find(b => b.Category == category);
        if (existing != null)
        {
            Debug.Log($"[Character] Se reemplaza buff {existing.Type} por {type} en categoría {category}");
            existing.CleanUp();
            activeBuffs.Remove(existing);
            HUD?.RemoveBuffIcon(existing.Type);
        }

        activeBuffs.Add(newBuff);

        // Mostrar ícono
        Sprite icon = StatusEffectIconDatabase.GetBuffIcon(type);
        if (HUD == null)
        {
            Debug.LogWarning($"[Character] HUD es NULL para {characterName}");
        }
        else
        {
            Debug.Log($"[Character] HUD existe, se intenta agregar icono de {type}");
            HUD.AddBuffIcon(type, icon, duration);
        }
    }

    public virtual IEnumerator OnTurnStart()
    {
        ApplyDebuffsAtTurnStart();
        yield return null;
    }

    public virtual IEnumerator PerformAction()
    {
        yield return null;
    }

    public virtual IEnumerator OnTurnEnd()
    {
        ApplyBuffsAtTurnEnd();
        yield return null;
    }

   public void ApplyDebuffsAtTurnStart()
    {
        List<Debuff> expired = new();

        foreach (var debuff in activeDebuffs)
        {
            debuff.ApplyTurnEffect();

            if (HUD != null)
                HUD.UpdateDebuffDuration(debuff.Type, debuff.Duration);

            if (debuff.IsExpired())
                expired.Add(debuff);
        }

        foreach (var debuff in expired)
        {
            debuff.CleanUp();
            activeDebuffs.Remove(debuff);
            HUD?.RemoveDebuffIcon(debuff.Type);
        }
    }
    
    public void ApplyBuffsAtTurnEnd()
    {
        List<Buff> expired = new();

        foreach (var buff in activeBuffs)
        {
            if (buff.WasJustApplied)
            {
                // Lo marcamos como "ya aplicado", pero no reducimos duración todavía
                buff.WasJustApplied = false;
                continue;
            }

            buff.Duration--;
            Debug.Log($"[Buff] {buff.Type} reduce duración a {buff.Duration}");

            HUD?.UpdateBuffDuration(buff.Type, buff.Duration);

            if (buff.IsExpired())
                expired.Add(buff);
        }

        foreach (var buff in expired)
        {
            buff.CleanUp();
            activeBuffs.Remove(buff);
            HUD?.RemoveBuffIcon(buff.Type);
            Debug.Log($"[Character] Buff {buff.Type} ha expirado al final del turno.");
        }
    }

    

    public IEnumerator MoveForward(float distance = 4f, float duration = 0.1f)
    {
        Transform spriteTransform = spriteRenderer.transform;

        if (originalLocalPosition == null)
            originalLocalPosition = spriteTransform.localPosition;

        Vector3 from = spriteTransform.localPosition;
        Vector3 to = from + Vector3.right * distance * (team == TeamType.Player ? 1 : -1);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteTransform.localPosition = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteTransform.localPosition = to;
    }

    public IEnumerator MoveBack(float duration = 0.1f)
    {
        if (originalLocalPosition == null)
            yield break;

        Transform spriteTransform = spriteRenderer.transform;
        Vector3 from = spriteTransform.localPosition;
        Vector3 to = originalLocalPosition.Value;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteTransform.localPosition = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteTransform.localPosition = to;
        originalLocalPosition = null;
    }

    public IEnumerator ShakeSprite(float intensity = 0.1f, float duration = 0.15f)
    {
        Transform spriteTransform = spriteRenderer.transform;
        Vector3 originalPos = spriteTransform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);

            spriteTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteTransform.localPosition = originalPos;
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        UpdateColor();
    }

    private void OnMouseExit()
    {
        isHovered = false;
        UpdateColor();
    }


    private void UpdateColor()
    {
        if (isSelected)
        {
            spriteRenderer.color = Color.yellow;
        }
        else if (isHovered)
        {
            spriteRenderer.color = Color.cyan; 
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }


    public void OnSelected()
    {
        if (isSelected) return;
        isSelected = true;
        UpdateColor();
    }

    public void OnDeselected()
    {
        if (!isSelected) return;
        isSelected = false;
        UpdateColor();
    }
}
