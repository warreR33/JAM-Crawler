using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AbilityButtonUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Button button;

    private AbilitySO abilityData;

    public void Setup(AbilitySO ability, bool hasEnergy, Action<AbilitySO> onClickCallback)
    {
        abilityData = ability;

        nameText.text = ability.abilityName;
        descriptionText.text = ability.description;


        button.interactable = hasEnergy;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallback?.Invoke(ability));
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}
