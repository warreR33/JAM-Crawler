using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    public static EnemyUIManager Instance;

    [Header("Contenedor de barras de vida")]
    public Transform healthBarContainer;  // Debe ser hijo del Canvas UI

    [Header("Prefab de la barra de vida")]
    public GameObject enemyHealthBarPrefab;

    private Dictionary<Character, EnemyHealthUI> activeHealthBars = new Dictionary<Character, EnemyHealthUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterEnemy(Character enemy)
    {
        if (activeHealthBars.ContainsKey(enemy))
            return;

        GameObject uiGO = Instantiate(enemyHealthBarPrefab, healthBarContainer);
        EnemyHealthUI healthUI = uiGO.GetComponent<EnemyHealthUI>();
        healthUI.Init(enemy);
        activeHealthBars.Add(enemy, healthUI);
    }

    public void UnregisterEnemy(Character enemy)
    {
        if (activeHealthBars.TryGetValue(enemy, out EnemyHealthUI healthUI))
        {
            Destroy(healthUI.gameObject);
            activeHealthBars.Remove(enemy);
        }
    }
}
