using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    public override IEnumerator OnTurnStart()
    {
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

        UIActionPanel.Instance.HidePlayerActionHUD();
        UIActionPanel.Instance.ClearCallbacks();
    }

    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        Debug.Log($"{characterName} gana {amount} de energía. Actual: {currentEnergy}/{maxEnergy}");
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
