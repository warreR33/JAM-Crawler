using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
