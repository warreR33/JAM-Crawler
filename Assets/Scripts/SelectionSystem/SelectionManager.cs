using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    private TeamType? allowedTargetTeam = null;

    private ISelectable currentSelection;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                ISelectable selectable = hit.collider.GetComponent<ISelectable>();
                Character character = hit.collider.GetComponent<Character>();

                if (character != null && UIActionPanel.Instance.IsTargeting)
                {
                    if (allowedTargetTeam != null && character.team != allowedTargetTeam.Value)
                    {
                        Debug.Log("Objetivo inválido: no se puede seleccionar este equipo.");
                        return;
                    }
                    UIActionPanel.Instance.OnCharacterClicked(character);
                    return;
                }

                // Selección normal
                if (selectable != null)
                {
                    if (selectable == currentSelection)
                    {
                        selectable.OnDeselected();
                        currentSelection = null;
                    }
                    else
                    {
                        currentSelection?.OnDeselected();
                        currentSelection = selectable;
                        currentSelection.OnSelected();
                    }
                }
            }
        }
    }

    public void SetAllowedTargetTeam(TeamType? team)
    {
        allowedTargetTeam = team;
    }

    public void ClearSelection()
    {
        if (currentSelection != null)
        {
            currentSelection.OnDeselected();
            Debug.Log($"Se deseleccionó: {currentSelection}");
        }

        allowedTargetTeam = null;
        currentSelection = null;
    }

    public ISelectable GetCurrentSelection() => currentSelection;
}
