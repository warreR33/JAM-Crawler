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

    [Header("HUD Prefabs")]
    public GameObject playerHUDPrefab;
    public GameObject enemyHUDPrefab;

    [Header("HUD Containers")]
    public Transform hudContainer; 

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
                GameObject characterGO = Instantiate(spawn.characterPrefab, spawn.position.position, Quaternion.identity);
                Character character = characterGO.GetComponent<Character>();

                if (character != null)
                {
                    GameObject hudPrefab = character.team == TeamType.Player ? playerHUDPrefab : enemyHUDPrefab;
                    GameObject hudGO = Instantiate(hudPrefab, hudContainer);

                    CharacterHUD hud = hudGO.GetComponent<CharacterHUD>();
                    if (hud != null)
                    {
                        bool isPlayer = character.team == TeamType.Player;
                        hud.Init(character, isPlayer);
                    }
                }
            }
        }
    }

}
