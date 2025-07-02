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

    [Range(0f, 1f)]
    public float critChance = 0.1f;       
    public float critMultiplier = 1.5f;   

    public bool IsAlive => currentHP > 0;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSelected = false;
    private bool isHovered = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        Debug.Log($"{characterName} recibe {amount} de daño. HP restante: {currentHP}");
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log($"{characterName} recupera {amount} de vida. HP actual: {currentHP}");
    }

    public virtual IEnumerator OnTurnStart()
    {
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
