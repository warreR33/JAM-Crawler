using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public AbilitySO basicAttack;
    public AbilitySO[] abilities = new AbilitySO[3];

    public System.Action<int, int> OnEnergyChanged;


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

    public IEnumerator UseAbility(AbilitySO ability, Character target)
    {
        if (ability == null) yield break;

        if (team == TeamType.Player)
            yield return StartCoroutine(MoveForward());

        yield return StartCoroutine(ability.ActivateRoutine(this, target));

        if (team == TeamType.Player)
            yield return StartCoroutine(MoveBack());

        SpendEnergy(ability.energyCost);

        if (Animator != null)
            Animator.SetTrigger("Idle");

        OnPlayerActionCompleted?.Invoke();
    }

    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
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
