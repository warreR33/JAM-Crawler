using UnityEngine;
using System.Collections.Generic;

public class CombatDataManager : MonoBehaviour
{
    public static CombatDataManager Instance { get; private set; }

    public List<GameObject> enemiesToSpawn = new();

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

    public void SetEncounter(GameObject[] enemies)
    {
        enemiesToSpawn.Clear();
        enemiesToSpawn.AddRange(enemies);
    }
}
