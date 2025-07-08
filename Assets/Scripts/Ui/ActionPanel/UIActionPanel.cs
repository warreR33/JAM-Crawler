using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionPanel : MonoBehaviour
{
    public static UIActionPanel Instance;

    [Header("Main Action Panel")]
    public GameObject actionCategoryPanel;
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button endTurnButton;

    [Header("Cancel Panel")]
    public GameObject cancelPanel;
    public Button cancelButton;

    [Header("Abilities Panel (Prefabs)")]
    public GameObject abilityPanel;
    public Transform abilityPanelContainer;
    public GameObject abilityButtonPrefab;

    private List<AbilityButtonUI> abilityButtonInstances = new List<AbilityButtonUI>();
    private System.Action onEndTurn;
    private System.Action<Character> onTargetSelected;

    private PlayerCharacter currentPlayer;
    public bool IsTargeting { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        actionCategoryPanel.SetActive(false);
        cancelPanel.SetActive(false);
        abilityPanel.SetActive(false);

        endTurnButton.onClick.AddListener(OnEndTurnPressed);

        attackButton.onClick.AddListener(() =>
        {
            if (currentPlayer == null || currentPlayer.basicAttack == null) return;

            currentPlayer.Animator.SetTrigger("Guard");

            EnterTargetingMode((Character target) =>
            {
                StartCoroutine(BasicAttackSequence(target));
            });

            SelectionManager.Instance.SetAllowedTargetTeam(TeamType.Enemy);
        });

        skillButton.onClick.AddListener(ShowAbilityPanel);

        itemButton.onClick.AddListener(() =>
        {
            Debug.Log("Uso de objetos no implementado.");
        });

        cancelButton.onClick.AddListener(ExitTargetingMode);
    }

    // === ATAQUE BÁSICO ===
    private IEnumerator BasicAttackSequence(Character target)
    {
        ExitTargetingMode();
        HidePlayerActionHUD();

        SelectionManager.Instance.SetAllowedTargetTeam(TeamType.Enemy);
        yield return currentPlayer.UseAbility(currentPlayer.basicAttack, target);
        yield return new WaitForSeconds(0.5f);

        onEndTurn?.Invoke();
    }

    // === HABILIDADES ===
    private void ShowAbilityPanel()
    {
        if (currentPlayer == null) return;

        actionCategoryPanel.SetActive(false);
        cancelPanel.SetActive(true);
        abilityPanel.SetActive(true);

        foreach (var b in abilityButtonInstances)
            Destroy(b.gameObject);
        abilityButtonInstances.Clear();

        foreach (var ability in currentPlayer.abilities)
        {
            if (ability == null) continue;

            GameObject go = Instantiate(abilityButtonPrefab, abilityPanelContainer);
            AbilityButtonUI buttonUI = go.GetComponent<AbilityButtonUI>();
            bool hasEnergy = currentPlayer.currentEnergy >= ability.energyCost;

            buttonUI.Setup(ability, hasEnergy, (selectedAbility) =>
            {
                currentPlayer.Animator.SetTrigger("Guard");

                if (selectedAbility.isArea)
                {
                    // Ejecutar directamente si es en área
                    HidePlayerActionHUD();
                    cancelPanel.SetActive(false);
                    abilityPanel.SetActive(false);
                    StartCoroutine(ExecuteAreaAbility(selectedAbility));
                }
                else
                {
                    EnterTargetingMode((Character target) =>
                    {
                        StartCoroutine(UseAbilitySequence(selectedAbility, target));
                    });

                    // Limitar selección según tipo de habilidad
                    if (selectedAbility.canTargetAllies && selectedAbility.canTargetEnemies)
                        SelectionManager.Instance.SetAllowedTargetTeam(null);
                    else if (selectedAbility.canTargetAllies)
                        SelectionManager.Instance.SetAllowedTargetTeam(TeamType.Player);
                    else if (selectedAbility.canTargetEnemies)
                        SelectionManager.Instance.SetAllowedTargetTeam(TeamType.Enemy);
                    else
                        SelectionManager.Instance.SetAllowedTargetTeam(null);

                    abilityPanel.SetActive(false);
                }
            });

            abilityButtonInstances.Add(buttonUI);
        }
    }

    private IEnumerator ExecuteAreaAbility(AbilitySO ability)
    {
        ExitTargetingMode(); 
        HidePlayerActionHUD();

        yield return currentPlayer.UseAbility(ability, null); 
        yield return new WaitForSeconds(0.3f);

        onEndTurn?.Invoke();
    }

    private List<Character> FindCharactersByTeam(TeamType team)
    {
        List<Character> list = new();
        foreach (Character c in GameObject.FindObjectsOfType<Character>())
        {
            if (c.team == team && c.IsAlive)
                list.Add(c);
        }
        return list;
    }

    private IEnumerator UseAbilitySequence(AbilitySO ability, Character target)
    {
        ExitTargetingMode();
        HidePlayerActionHUD();

        yield return currentPlayer.UseAbility(ability, target);
        yield return new WaitForSeconds(0.5f);

        onEndTurn?.Invoke();
    }

    // === TARGETING ===
    private void EnterTargetingMode(System.Action<Character> callback)
    {
        SetAbilityCallback(callback);
        actionCategoryPanel.SetActive(false);
        cancelPanel.SetActive(true);
        abilityPanel.SetActive(false);
    }

    private void ExitTargetingMode()
    {
        IsTargeting = false;
        onTargetSelected = null;

        cancelPanel.SetActive(false);
        abilityPanel.SetActive(false);
        actionCategoryPanel.SetActive(true);

        SelectionManager.Instance.ClearSelection();
        SelectionManager.Instance.SetAllowedTargetTeam(null);

        if (currentPlayer?.Animator != null)
            currentPlayer.Animator.SetTrigger("Idle");
    }

    public void SetAbilityCallback(System.Action<Character> callback)
    {
        onTargetSelected = callback;
        IsTargeting = true;
        SelectionManager.Instance.ClearSelection();
    }

    public void OnCharacterClicked(Character target)
    {
        if (!IsTargeting || onTargetSelected == null)
            return;

        IsTargeting = false;
        onTargetSelected.Invoke(target);
        onTargetSelected = null;

        if (target is ISelectable selectableTarget)
            selectableTarget.OnDeselected();

        SelectionManager.Instance.ClearSelection();
    }

    // === HUD VISIBILIDAD ===
    public void ShowPlayerActionHUD(PlayerCharacter character)
    {
        currentPlayer = character;

        actionCategoryPanel.SetActive(true);
        cancelPanel.SetActive(false);
        abilityPanel.SetActive(false);
    }

    public void HidePlayerActionHUD()
    {
        actionCategoryPanel.SetActive(false);
        cancelPanel.SetActive(false);
        abilityPanel.SetActive(false);
    }

    public void SetEndTurnCallback(System.Action callback)
    {
        onEndTurn = callback;
    }

    public void ClearCallbacks()
    {
        onEndTurn = null;
        onTargetSelected = null;
        IsTargeting = false;
        currentPlayer = null;
    }

    private void OnEndTurnPressed()
    {
        onEndTurn?.Invoke();
    }
}
