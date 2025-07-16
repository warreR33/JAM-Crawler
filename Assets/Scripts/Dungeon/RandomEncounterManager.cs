using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEncounterManager : MonoBehaviour
{
    [Header("Configuración")]
    public int stepsBeforeMin = 10;
    public float encounterChance = 0.1f;

    [SerializeField] private int steps = 0;

    public EncounterZone currentZone;

    public void RegisterStep()
    {
        if (currentZone == null || !currentZone.IsActive)
            return;

        steps++;

        if (steps >= stepsBeforeMin)
        {
            float roll = Random.value;
            if (roll < encounterChance)
            {
                TriggerEncounter();
                steps = 0;
            }
        }
    }

    public void SetStepCount(int count)
    {
        steps = count;
    }

    private void TriggerEncounter()
    {
        steps = 0;
        Debug.Log($"¡Encuentro en zona {currentZone.zoneID}!");
        currentZone.RegisterEncounter();

        var randomGroup = currentZone.possibleEncounters[Random.Range(0, currentZone.possibleEncounters.Count)];
        CombatDataManager.Instance.SetEncounter(randomGroup.enemies);

        // Guardar estado actual
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        var movement = playerGO.GetComponent<PlayerMovement2D>();
        Vector3 playerPosition = playerGO.transform.position;

        GameStateManager.Instance.SaveDungeonState(playerPosition, currentZone, steps);

        GameStateManager.Instance.SaveAllZones();
        SceneManager.LoadScene("CombatTest");
    }
}
