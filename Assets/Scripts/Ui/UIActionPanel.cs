using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIActionPanel : MonoBehaviour
{
    public static UIActionPanel Instance;


    [Header("Current Player Info")]
    public GameObject currentPlayerInfoPanel;
    public Image currentPlayerIcon;
    public Image hpFillImage;
    public Image energyFillImage;

    public TMP_Text hpText;
    public TMP_Text energyText;

    [Header("Turn UI")]
    public GameObject playerActionHUD;
    public Button endTurnButton;
    public Button actionButton1;
    public Button actionButton2;
    public Button actionButton3;
    public Button actionButton4;

    private System.Action onEndTurn;

    private System.Action<Character> onTargetSelected;
    public bool isTargeting = false;

    public bool IsTargeting => isTargeting;

    private PlayerCharacter currentPlayer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        playerActionHUD.SetActive(false);

        endTurnButton.onClick.AddListener(OnEndTurnPressed);

        actionButton1.onClick.AddListener(() =>
        {
            SetAbilityCallback((Character target) =>
            {
                Debug.Log("Ejecutar habilidad básica contra " + target.characterName);

                if (PlayerCharacter.Current != null)
                {
                    PlayerCharacter.Current.PerformBasicAttack(target);
                }

                actionButton1.interactable = false;
            });
        });
    }


    private void SetupAbilityButtons(PlayerCharacter player)
    {
        var buttons = new Button[] { actionButton1, actionButton2, actionButton3, actionButton4 };

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < player.abilities.Length)
            {
                AbilitySO ability = player.abilities[i];
                Button btn = buttons[i];
                btn.gameObject.SetActive(true);

                TextMeshProUGUI textComponent = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = ability.abilityName;
                }

                int index = i;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    SetAbilityCallback((Character target) =>
                    {
                        ability.Activate(player, target);
                        player.SpendEnergy(ability.energyCost);
                        player.OnPlayerActionCompleted?.Invoke();
                    });

                    // Borra la selección previa
                    SelectionManager.Instance.ClearSelection();
                });
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetAbilityCallback(System.Action<Character> callback)
    {
        onTargetSelected = callback;
        isTargeting = true;

        SelectionManager.Instance.ClearSelection();
        Debug.Log("Modo selección de objetivo activado");
    }

    public void OnCharacterClicked(Character target)
    {
        if (!isTargeting || onTargetSelected == null)
            return;

        isTargeting = false;

        onTargetSelected.Invoke(target);
        onTargetSelected = null;

            if (target is ISelectable selectableTarget)
        selectableTarget.OnDeselected();


        SelectionManager.Instance.ClearSelection();
    }

    public void SetupAbilities(PlayerCharacter character)
    {
        actionButton1.GetComponentInChildren<TMPro.TMP_Text>().text = character.basicAttack != null ? character.basicAttack.abilityName : "Atacar";

        Image iconImage = actionButton1.GetComponentInChildren<Image>();
        if (iconImage != null && character.basicAttack.icon != null)
        {
            iconImage.sprite = character.basicAttack.icon;
            iconImage.enabled = true;
        }

        for (int i = 0; i < character.abilities.Length; i++)
        {
            var button = i switch
            {
                0 => actionButton2,
                1 => actionButton3,
                2 => actionButton4,
                _ => null
            };

            if (button != null && character.abilities[i] != null)
            {
                button.GetComponentInChildren<TMPro.TMP_Text>().text = character.abilities[i].abilityName;
            }
        }
    }

    public void ShowPlayerActionHUD(PlayerCharacter character)
    {
        playerActionHUD.SetActive(true);
        SetupAbilities(character);
        actionButton1.interactable = true;
        actionButton2.interactable = true;
        actionButton3.interactable = true;
        actionButton4.interactable = true;

    }

    public void ShowCurrentPlayerInfo(PlayerCharacter player)
    {
        currentPlayerInfoPanel.SetActive(true);

        currentPlayerIcon.sprite = player.icon;

        if (hpFillImage != null)
            hpFillImage.fillAmount = (float)player.currentHP / player.maxHP;

        if (energyFillImage != null)
            energyFillImage.fillAmount = (float)player.currentEnergy / player.maxEnergy;

        if (hpText != null)
            hpText.text = $"{player.currentHP}/{player.maxHP}";

        if (energyText != null)
            energyText.text = $"{player.currentEnergy}/{player.maxEnergy}";
    }


    public void HideCurrentPlayerInfo()
    {
        currentPlayerInfoPanel.SetActive(false);
    }

    public void HidePlayerActionHUD()
    {
        playerActionHUD.SetActive(false);
    }

    public void SetEndTurnCallback(System.Action callback)
    {
        onEndTurn = callback;
    }

    public void ClearCallbacks()
    {
        onEndTurn = null;
        onTargetSelected = null;
        isTargeting = false;
    }

    private void OnEndTurnPressed()
    {
        onEndTurn?.Invoke();
    }

    // === Turn Info ===

    public void SetCurrentTurn(Character current)
    {
        Debug.Log($"Turno de: {current.characterName} ({current.team})");
    }

    public void SetUpcomingTurn(Character next)
    {
        Debug.Log($"Siguiente turno: {next.characterName}");
    }
}
