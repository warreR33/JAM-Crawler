using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

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

                // Si estamos en modo de selección de objetivo para una habilidad:
                if (character != null && UIActionPanel.Instance.IsTargeting)
                {
                    UIActionPanel.Instance.OnCharacterClicked(character);
                    return; // Evita continuar con selección normal
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

    public void ClearSelection()
    {
        if (currentSelection != null)
        {
            currentSelection.OnDeselected();
            Debug.Log($"Se deseleccionó: {currentSelection}");
        }

        currentSelection = null;
    }

    public ISelectable GetCurrentSelection() => currentSelection;
}
