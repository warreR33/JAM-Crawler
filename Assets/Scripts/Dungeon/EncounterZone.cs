using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class EnemyGroup
{
    public GameObject[] enemies; // máximo 3 enemigos
}

[RequireComponent(typeof(BoxCollider2D))]
public class EncounterZone : MonoBehaviour
{
    public string zoneID;
    public int maxEncounters = 3;
    private int currentEncounters = 0;

    public List<EnemyGroup> possibleEncounters;

    public bool IsActive => currentEncounters < maxEncounters;

    public void RegisterEncounter()
    {
        currentEncounters++;
    }

    public int RemainingEncounters
    {
        get { return maxEncounters - currentEncounters; }
        set { currentEncounters = Mathf.Clamp(maxEncounters - value, 0, maxEncounters); }
    }

}
