using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public AbilitySO basicAttack;
    public AbilitySO[] abilities = new AbilitySO[3];

    public static PlayerCharacter Current;

    public GameObject smallHudObject;

    public System.Action<int, int> OnEnergyChanged;

    public override IEnumerator OnTurnStart()
    {
        Current = this;

        // Ocultamos HUD pequeño y mostramos el grande
        if (smallHudObject != null)
            smallHudObject.SetActive(false);

        UIActionPanel.Instance.ShowCurrentPlayerInfo(this);

        GainEnergy(1);
        yield return null;
    }

    public override IEnumerator PerformAction()
    {
        UIActionPanel.Instance.ShowPlayerActionHUD(this);

        bool waiting = true;
        UIActionPanel.Instance.SetEndTurnCallback(() => waiting = false);

        while (waiting)
            yield return null;

        // Al terminar la acción, ocultamos el HUD grande y volvemos a mostrar el pequeño
        UIActionPanel.Instance.HidePlayerActionHUD();
        UIActionPanel.Instance.HideCurrentPlayerInfo();

        if (smallHudObject != null)
            smallHudObject.SetActive(true);

        UIActionPanel.Instance.ClearCallbacks();
    }

    public void PerformBasicAttack(Character target)
    {
        if (basicAttack.visualEffectPrefab != null)
        {
            GameObject fx = Instantiate(basicAttack.visualEffectPrefab, target.transform.position, Quaternion.identity); 
        }

        float rawDamage = power * 1.2f;
        float finalDamage = Mathf.Max(0, rawDamage - target.defense);
        target.TakeDamage(Mathf.RoundToInt(finalDamage));
        Debug.Log($"{characterName} ataca a {target.characterName} causando {finalDamage} de daño");
    }

    public void GainEnergy(int amount)
    {

        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        Debug.Log($"{characterName} gana {amount} de energía. Actual: {currentEnergy}/{maxEnergy}");
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        UIActionPanel.Instance.ShowCurrentPlayerInfo(this);
    }

    public bool SpendEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }

        Debug.Log($"{characterName} no tiene suficiente energía ({currentEnergy}/{amount})");
        return false;
    }
}
