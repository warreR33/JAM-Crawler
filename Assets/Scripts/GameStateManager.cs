using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public Vector3 playerPositionInDungeon;
    public string dungeonSceneName = "Dungeon";
    public EncounterZone lastZone;
    public int stepCounter = 0;
    public Dictionary<string, int> remainingEncountersPerZone = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveDungeonState(Vector3 playerPos, EncounterZone zone, int steps)
    {
        playerPositionInDungeon = playerPos;
        lastZone = zone;
        stepCounter = steps;
    }

    public void LoadDungeonScene()
    {
        SceneManager.LoadScene(dungeonSceneName);
    }

    public void SaveZoneEncounterCount(EncounterZone zone)
    {
        if (zone == null || string.IsNullOrEmpty(zone.zoneID)) return;

        remainingEncountersPerZone[zone.zoneID] = zone.RemainingEncounters;
    }

    public void SaveAllZones()
    {
        EncounterZone[] allZones = GameObject.FindObjectsOfType<EncounterZone>();

        foreach (var zone in allZones)
        {
            if (!string.IsNullOrEmpty(zone.zoneID))
            {
                remainingEncountersPerZone[zone.zoneID] = zone.RemainingEncounters;
            }
        }
    }

    public void LoadZoneEncounterCount(EncounterZone zone)
    {
        if (zone == null || string.IsNullOrEmpty(zone.zoneID)) return;

        if (remainingEncountersPerZone.TryGetValue(zone.zoneID, out int count))
        {
            zone.RemainingEncounters = count;
        }
    }
}
