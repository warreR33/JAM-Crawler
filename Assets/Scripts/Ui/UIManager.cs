using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Turn Order Icons HUD")]
    public GameObject turnHUDPrefab;
    public Transform turnHUDContainer;
    public GameObject playerTurnHUDPrefab;
    public GameObject enemyTurnHUDPrefab;

    [Header("Turn Circle Rotation")]
    public Transform turnCircleParent;
    public float rotationSpeed = 300f;
    private int currentHighlightedIndex = -1;

    [Header("Turn Circle Settings")]
    [SerializeField] private float radius = 150f;
    [SerializeField] private float totalAngle = 360f;

    private List<TurnHUD> currentTurnHUDs = new();

    public IReadOnlyList<TurnHUD> CurrentTurnHUDs => currentTurnHUDs;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTurnHUDs(List<Character> characters)
    {
        foreach (Transform child in turnHUDContainer)
            Destroy(child.gameObject);

        currentTurnHUDs.Clear();

        int count = characters.Count;
        float angleStep = totalAngle / count;
        float startAngle = (360f - totalAngle) / 2f;

        for (int i = 0; i < count; i++)
        {
            GameObject prefabToUse = characters[i].team == TeamType.Player
                ? playerTurnHUDPrefab
                : enemyTurnHUDPrefab;

            GameObject hudGO = Instantiate(prefabToUse, turnHUDContainer);
            TurnHUD hud = hudGO.GetComponent<TurnHUD>();

            if (hud != null)
            {
                hud.SetData(characters[i].icon, characters[i].speed + characters[i].initiativeRoll);
                currentTurnHUDs.Add(hud);

                float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

                hud.transform.localPosition = pos;
                hud.transform.localRotation = Quaternion.identity;
                hud.SetOriginalPosition(pos);
            }
        }
    }

    public void HighlightCurrentTurn(int index)
    {
        currentHighlightedIndex = index;
        RotateCircleToHighlight(index);
    }

    public void RotateCircleToHighlight(int index, float radius = 150f)
    {
        int count = currentTurnHUDs.Count;
        if (count == 0) return;

        float angleStep = 360f / count;
        float targetAngle = 270f - (angleStep * index);

        StopAllCoroutines();
        StartCoroutine(RotateContainerCoroutine(targetAngle, radius));

    }

    private IEnumerator RotateContainerCoroutine(float targetAngle, float radius)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        float startAngle = turnHUDContainer.localEulerAngles.z;
        if (startAngle > 180) startAngle -= 360;

        // Diferencia angular mínima entre start y target
        float angleDifference = Mathf.DeltaAngle(startAngle, targetAngle);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Interpolamos sumando la diferencia proporcional
            float angle = startAngle + angleDifference * t;

            turnHUDContainer.localEulerAngles = new Vector3(0, 0, angle);

            // Invertimos rotación de los iconos para que no roten con el container
            foreach (var hud in currentTurnHUDs)
                hud.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            yield return null;
        }

        // Al final aseguramos posición final exacta
        turnHUDContainer.localEulerAngles = new Vector3(0, 0, targetAngle);

        foreach (var hud in currentTurnHUDs)
            hud.transform.localRotation = Quaternion.Euler(0, 0, -targetAngle);

        for (int i = 0; i < currentTurnHUDs.Count; i++)
        {
            if (i == currentHighlightedIndex)
                currentTurnHUDs[i].HighlightTurn();
            else
                currentTurnHUDs[i].ResetPosition();
        }
    }

}
