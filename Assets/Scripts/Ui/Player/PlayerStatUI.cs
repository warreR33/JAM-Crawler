using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
  [Header("UI References")]
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text energyText;
    public TMP_Text HPText;
    public Image healthBarFill;

    private PlayerCharacter target;

    public void Setup(PlayerCharacter character)
    {
        target = character;
        icon.sprite = character.GetComponent<SpriteRenderer>().sprite; 
        nameText.text = character.characterName;

        UpdateStats(); 
    }

    public void UpdateStats()
    {
        if (target == null) return;

        energyText.text = $"Energía: {target.currentEnergy}/{target.maxEnergy}";
        HPText.text = $"{target.currentHP}/{target.maxHP}";

        float healthPercent = (float)target.currentHP / target.maxHP;
        healthBarFill.fillAmount = healthPercent;
    }
}
