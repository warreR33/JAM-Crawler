using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform position;
        public GameObject characterPrefab;
    }

    [Header("Spawn Points")]
    public SpawnPoint[] playerSpawns;
    public SpawnPoint[] enemySpawns;

    private void Start()
    {
        SpawnCharacters(playerSpawns);
        SpawnCharacters(enemySpawns);
    }

    private void SpawnCharacters(SpawnPoint[] spawns)
    {
        foreach (var spawn in spawns)
        {
            if (spawn.characterPrefab != null && spawn.position != null)
            {
                Instantiate(spawn.characterPrefab, spawn.position.position, Quaternion.identity);
            }
        }
    }
}
