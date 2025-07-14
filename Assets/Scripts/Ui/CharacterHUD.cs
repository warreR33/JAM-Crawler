using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterHUD : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image energyFill;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI nameText;

    private Character target;
    private Camera mainCamera;
    private RectTransform rectTransform;

    private Vector3 screenOffset = new Vector3(0, 100f, 0);
    private float currentHPFill = 1f;
    private float currentEnergyFill = 1f;

    private bool showEnergy = false;

    [Header("Estados")]
    [SerializeField] private Transform statusIconContainer;
    [SerializeField] private StatusEffectIcon statusIconPrefab;

    private Dictionary<DebuffType, StatusEffectIcon> debuffIcons = new();
    private Dictionary<BuffType, StatusEffectIcon> buffIcons = new();

    public void Init(Character character, bool isPlayer)
    {
        target = character;
        showEnergy = isPlayer;

        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();

        if (nameText != null)
            nameText.text = character.characterName;

        if (!showEnergy)
        {
            energyFill?.gameObject.SetActive(false);
            energyText?.gameObject.SetActive(false);
            nameText?.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (target == null) return;
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.transform.position);
        rectTransform.position = screenPos + screenOffset;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (target.maxHP <= 0) return;

        float hpTarget = (float)target.currentHP / target.maxHP;
        currentHPFill = Mathf.Lerp(currentHPFill, hpTarget, Time.deltaTime * 10f);
        healthFill.fillAmount = currentHPFill;

        hpText.text = $"{target.currentHP}/{target.maxHP}";

        if (showEnergy && energyFill != null && energyText != null)
        {
            float energyTarget = (float)target.currentEnergy / target.maxEnergy;
            currentEnergyFill = Mathf.Lerp(currentEnergyFill, energyTarget, Time.deltaTime * 10f);
            energyFill.fillAmount = currentEnergyFill;
            energyText.text = $"{target.currentEnergy}/{target.maxEnergy}";
        }
    }
    
    public void AddDebuffIcon(DebuffType type, Sprite iconSprite, int duration)
    {
        if (debuffIcons.ContainsKey(type))
        {
            debuffIcons[type].SetDuration(duration);
        }
        else
        {
            var icon = Instantiate(statusIconPrefab, statusIconContainer);
            icon.Init(iconSprite, duration);
            debuffIcons.Add(type, icon);
        }   
    }

    public void AddBuffIcon(BuffType type, Sprite iconSprite, int duration)
    {
        
        Debug.Log($"[AddBebuffIcon] {type} - duración: {duration} - sprite: {(iconSprite != null ? iconSprite.name : "null")}");

        if (buffIcons.ContainsKey(type))
        {
            buffIcons[type].SetDuration(duration);
        }
        else
        {
            var icon = Instantiate(statusIconPrefab, statusIconContainer);
            icon.Init(iconSprite, duration);
            buffIcons.Add(type, icon);
        }
    }

    public void RemoveDebuffIcon(DebuffType type)
    {
        if (debuffIcons.TryGetValue(type, out var icon))
        {
            Destroy(icon.gameObject);
            debuffIcons.Remove(type);
        }
    }

    public void RemoveBuffIcon(BuffType type)
    {
        if (buffIcons.TryGetValue(type, out var icon))
        {
            Destroy(icon.gameObject);
            buffIcons.Remove(type);
        }
    }

    public void UpdateDebuffDuration(DebuffType category, int newDuration)
    {
        if (debuffIcons.TryGetValue(category, out var icon))
        {
            icon.SetDuration(newDuration);
        }
    }

    public void UpdateBuffDuration(BuffType  category, int newDuration)
    {
        if (buffIcons.TryGetValue(category, out var icon))
        {
            icon.SetDuration(newDuration);
        }
    }

    private StatusEffectIcon GetDebuffIconByCategory(DebuffCategory category)
    {
        foreach (var kvp in debuffIcons)
        {
            if (DebuffUtils.GetCategory(kvp.Key) == category)
                return kvp.Value;
        }
        return null;
    }

    private DebuffType? GetDebuffKeyByCategory(DebuffCategory category)
    {
        foreach (var kvp in debuffIcons)
        {
            if (DebuffUtils.GetCategory(kvp.Key) == category)
                return kvp.Key;
        }
        return null;
    }

    private StatusEffectIcon GetBuffIconByCategory(BuffCategory category)
    {
        foreach (var kvp in buffIcons)
        {
            if (BuffUtils.GetCategory(kvp.Key) == category)
                return kvp.Value;
        }
        return null;
    }

    private BuffType? GetBuffKeyByCategory(BuffCategory category)
    {
        foreach (var kvp in buffIcons)
        {
            if (BuffUtils.GetCategory(kvp.Key) == category)
                return kvp.Key;
        }
        return null;
    }
}
