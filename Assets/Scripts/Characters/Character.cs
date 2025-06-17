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

    [Header("Stats")]
    public string characterName;

    public int speed = 10;
    [HideInInspector] public int initiativeRoll = 0;

    public int maxHP = 100;
    public int currentHP = 100;

    public int maxAP = 3;
    public int currentAP = 3;

    public TeamType team;

    public bool IsAlive => currentHP > 0;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isSelected = false;

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
        currentAP = maxAP;
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
    


    public void OnSelected()
    {
        if (isSelected) return;

        isSelected = true;
        spriteRenderer.color = Color.yellow; 
    }

    public void OnDeselected()
    {
        if (!isSelected) return;

        isSelected = false;
        spriteRenderer.color = originalColor;
    }
}
