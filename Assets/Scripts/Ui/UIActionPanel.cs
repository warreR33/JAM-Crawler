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
    public Button actionButton1; // Basic attack
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
            if (PlayerCharacter.Current == null || PlayerCharacter.Current.basicAttack == null)
                return;

            SetAbilityCallback((Character target) =>
            {
                AbilitySO basic = PlayerCharacter.Current.basicAttack;

                if (PlayerCharacter.Current.currentEnergy >= basic.energyCost)
                {
                    basic.Activate(PlayerCharacter.Current, target);
                    PlayerCharacter.Current.SpendEnergy(basic.energyCost);
                    PlayerCharacter.Current.OnPlayerActionCompleted?.Invoke();
                }
                else
                {
                    Debug.Log($"{PlayerCharacter.Current.characterName} no tiene energía suficiente para usar el ataque básico.");
                }

                actionButton1.interactable = false;
            });
        });
    }

    private void SetupAbilityButtons(PlayerCharacter player)
    {
        currentPlayer = player;
        var buttons = new Button[] { actionButton2, actionButton3, actionButton4 };

        for (int i = 0; i < buttons.Length; i++)
        {
            Button btn = buttons[i];
            if (i < player.abilities.Length && player.abilities[i] != null)
            {
                AbilitySO ability = player.abilities[i];
                btn.gameObject.SetActive(true);

                var textComponent = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                    textComponent.text = ability.abilityName;

                btn.onClick.RemoveAllListeners();

                bool hasEnergy = player.currentEnergy >= ability.energyCost;
                btn.interactable = hasEnergy;

                if (hasEnergy)
                {
                    btn.onClick.AddListener(() =>
                    {
                        SetAbilityCallback((Character target) =>
                        {
                            ability.Activate(player, target);
                            player.SpendEnergy(ability.energyCost);
                            player.OnPlayerActionCompleted?.Invoke();
                        });

                        SelectionManager.Instance.ClearSelection();
                    });
                }
                else
                {
                    btn.onClick.AddListener(() =>
                    {
                        Debug.Log($"{player.characterName} no tiene energía suficiente para usar {ability.abilityName}.");
                    });
                }
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    private void RefreshAbilityButtons(int current, int max)
    {
        if (currentPlayer != null)
        {
            SetupAbilityButtons(currentPlayer);
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
        actionButton1.GetComponentInChildren<TMP_Text>().text = character.basicAttack != null ? character.basicAttack.abilityName : "Atacar";

        Image iconImage = actionButton1.GetComponentInChildren<Image>();
        if (iconImage != null && character.basicAttack.icon != null)
        {
            iconImage.sprite = character.basicAttack.icon;
            iconImage.enabled = true;
        }

        var abilityTexts = new TMP_Text[] {
            actionButton2.GetComponentInChildren<TMP_Text>(),
            actionButton3.GetComponentInChildren<TMP_Text>(),
            actionButton4.GetComponentInChildren<TMP_Text>()
        };

        for (int i = 0; i < character.abilities.Length && i < abilityTexts.Length; i++)
        {
            if (character.abilities[i] != null)
            {
                abilityTexts[i].text = character.abilities[i].abilityName;
            }
        }
    }

    public void ShowPlayerActionHUD(PlayerCharacter character)
    {
        playerActionHUD.SetActive(true);

        // Suscribimos para actualizar botones al cambiar energía
        character.OnEnergyChanged += RefreshAbilityButtons;

        SetupAbilities(character);
        SetupAbilityButtons(character);
        actionButton1.interactable = true;
    }

    public void ShowCurrentPlayerInfo(PlayerCharacter player)
    {
        currentPlayerInfoPanel.SetActive(true);
        currentPlayer = player;

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

        if (currentPlayer != null)
        {
            currentPlayer.OnEnergyChanged -= RefreshAbilityButtons;
            currentPlayer = null;
        }
    }

    private void OnEndTurnPressed()
    {
        onEndTurn?.Invoke();
    }

    public void SetCurrentTurn(Character current)
    {
        Debug.Log($"Turno de: {current.characterName} ({current.team})");
    }

    public void SetUpcomingTurn(Character next)
    {
        Debug.Log($"Siguiente turno: {next.characterName}");
    }
}
