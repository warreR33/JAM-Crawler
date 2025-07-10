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
        Debug.Log($"{characterName} recibe {amount} de daño. HP restante: {currentHP}");

        StartCoroutine(ShakeSprite());
    }
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log($"{characterName} recupera {amount} de vida. HP actual: {currentHP}");
    }

    public void AddDebuff(DebuffType type, int duration)
    {
        Debuff newDebuff = new Debuff(type, duration, this);
        activeDebuffs.Add(newDebuff);
    }

    public void AddBuff(BuffType type, int duration)
    {
        Buff buff = new Buff(type, duration, this);
        activeBuffs.Add(buff);
    }

    public virtual IEnumerator OnTurnStart()
    {
        ApplyBuffsAtTurnStart();
        ApplyDebuffsAtTurnStart();
        yield return null;
    }

    public virtual IEnumerator PerformAction()
    {
        yield return null;
    }

    public virtual IEnumerator OnTurnEnd()
    {
        yield return null;
    }

    public void ApplyDebuffsAtTurnStart()
    {
        List<Debuff> expired = new List<Debuff>();

        foreach (Debuff debuff in activeDebuffs)
        {
            debuff.ApplyTurnEffect();

            if (debuff.IsExpired())
            {
                expired.Add(debuff);
            }
        }

        foreach (Debuff debuff in expired)
        {
            debuff.CleanUp();
            activeDebuffs.Remove(debuff);
        }
    }

    
    public void ApplyBuffsAtTurnStart()
    {
        List<Buff> expired = new List<Buff>();

        foreach (var buff in activeBuffs)
        {
            buff.ApplyTurnEffect();
            if (buff.IsExpired())
                expired.Add(buff);
        }

        foreach (var buff in expired)
        {
            buff.CleanUp();
            activeBuffs.Remove(buff);
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
