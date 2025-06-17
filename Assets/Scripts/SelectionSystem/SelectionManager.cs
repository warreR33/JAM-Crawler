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
        currentSelection?.OnDeselected();
        currentSelection = null;
    }

    public ISelectable GetCurrentSelection() => currentSelection;
}
