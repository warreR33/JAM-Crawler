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

        UIActionPanel.Instance.HidePlayerActionHUD();
        UIActionPanel.Instance.HideCurrentPlayerInfo();

        UIActionPanel.Instance.ClearCallbacks();
    }

    public IEnumerator UseAbility(AbilitySO ability, Character target)
    {
        if (ability == null || target == null) yield break;

        yield return StartCoroutine(CombatVisualFeedbackManager.Instance.PlayAbilityStartFX(ability.abilityName));
        yield return StartCoroutine(ability.ActivateRoutine(this, target));
        yield return StartCoroutine(CombatVisualFeedbackManager.Instance.EndAbilityFX());

        SpendEnergy(ability.energyCost);
        OnPlayerActionCompleted?.Invoke();
    }

    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
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
