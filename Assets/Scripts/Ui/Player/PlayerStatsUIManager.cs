using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUIManager : MonoBehaviour
{
    public GameObject playerPanelPrefab;
    public Transform panelContainer;

    private List<PlayerStatUI> activePanels = new();

    void Start()
    {
        SpawnAllPlayerPanels();
    }

    public void SpawnAllPlayerPanels()
    {
        PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();

        foreach (var player in players)
        {
            GameObject panelGO = Instantiate(playerPanelPrefab, panelContainer);
            PlayerStatUI panel = panelGO.GetComponent<PlayerStatUI>();
            panel.Setup(player);
            activePanels.Add(panel);
        }
    }

    void Update()
    {
        foreach (var panel in activePanels)
        {
            panel.UpdateStats();
        }
    }
}
