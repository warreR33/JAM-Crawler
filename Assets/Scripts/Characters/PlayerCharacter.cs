using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public override IEnumerator PerformAction()
    {
        UIActionPanel.Instance.ShowPlayerActionHUD(this);

        bool waiting = true;
        UIActionPanel.Instance.SetEndTurnCallback(() => waiting = false); // Se registra el callback

        while (waiting)
            yield return null;

        // Ocultar el panel cuando finaliza
        UIActionPanel.Instance.HidePlayerActionHUD();
        UIActionPanel.Instance.ClearCallbacks();
    }
}
