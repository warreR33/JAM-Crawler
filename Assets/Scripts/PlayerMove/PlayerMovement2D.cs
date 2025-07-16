using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public Transform spriteTransform;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    private Vector3Int currentTile;
    private Tilemap groundTilemap;
    private RandomEncounterManager encounterManager;

    private EncounterZone currentZone;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        groundTilemap = GameObject.Find("Floor").GetComponent<Tilemap>(); // ← Asegurate que el tilemap se llame así
        encounterManager = FindObjectOfType<RandomEncounterManager>();

        currentTile = groundTilemap.WorldToCell(rb.position);
    }

    void Start()
    {
        if (GameStateManager.Instance != null)
        {
            transform.position = GameStateManager.Instance.playerPositionInDungeon;

            currentZone = GameStateManager.Instance.lastZone;
            encounterManager.currentZone = currentZone;

            encounterManager.SetStepCount(GameStateManager.Instance.stepCounter);
            EncounterZone[] allZones = FindObjectsOfType<EncounterZone>();
            foreach (var zone in allZones)
            {
                GameStateManager.Instance.LoadZoneEncounterCount(zone);
            }
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement != Vector2.zero);
    }

    void FixedUpdate()
    {
        rb.velocity = movement.normalized * moveSpeed;

        // Calcular tile actual
        Vector3Int tilePos = groundTilemap.WorldToCell(rb.position);

        // Si cambió de tile, contamos un paso
        if (tilePos != currentTile)
        {
            currentTile = tilePos;
            encounterManager?.RegisterStep(); // ← solo suma pasos cuando cambia de tile
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EncounterZone zone = other.GetComponent<EncounterZone>();
        if (zone != null)
        {
            currentZone = zone;
            if (encounterManager != null)
                encounterManager.currentZone = zone;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EncounterZone>() == currentZone)
        {
            currentZone = null;
        }
    }
}
