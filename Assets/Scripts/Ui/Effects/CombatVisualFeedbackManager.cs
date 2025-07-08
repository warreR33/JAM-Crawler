using System.Collections;
using UnityEngine;
using TMPro;

public class CombatVisualFeedbackManager : MonoBehaviour
{
    public static CombatVisualFeedbackManager Instance;


    [Header("Ability Name Display")]
    [SerializeField] private GameObject abilityNamePanel;
    [SerializeField] private TMP_Text abilityNameText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        abilityNamePanel.SetActive(false);
    }


    public IEnumerator ShowAbilityName(string abilityName, float duration = 1f)
    {
        abilityNamePanel.SetActive(true);
        abilityNameText.text = abilityName;

        yield return new WaitForSeconds(duration);

        abilityNamePanel.SetActive(false);
    }


    public IEnumerator PlayAbilityStartFX(string abilityName)
    {
        yield return StartCoroutine(ShowAbilityName(abilityName, 1f));
    }

    public IEnumerator EndAbilityFX()
    {
        yield return new WaitForSeconds(0.4f);
    }
}
