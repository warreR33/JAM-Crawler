using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public override IEnumerator PerformAction()
    {
        UIActionPanel.Instance.ShowPlayerActionHUD(this);

        bool waiting = true;
        UIActionPanel.Instance.SetEndTurnCallback(() => waiting = false);

        while (waiting)
            yield return null;

        UIActionPanel.Instance.HidePlayerActionHUD();
        UIActionPanel.Instance.ClearCallbacks();
    }
}
