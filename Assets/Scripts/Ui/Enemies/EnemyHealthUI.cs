using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI hpText;

    private Character target;
    private Camera mainCamera;
    private RectTransform rectTransform;

    private Vector3 screenOffset = new Vector3(0, 100f, 0); 

    private float currentFill = 1f;


    public void Init(Character character)
    {
        target = character;
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        UpdateUI();
    }

    private void Update()
    {
        if (target == null) return;

        // Convertir posición world a pantalla
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.transform.position);

        // Agregar offset para que esté arriba del enemigo
        rectTransform.position = screenPos + screenOffset;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (target.maxHP <= 0) return;

        float targetFill = (float)target.currentHP / target.maxHP;
        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * 10f);
        fillImage.fillAmount = currentFill;

        hpText.text = $"{target.currentHP}/{target.maxHP}";
    }
}
