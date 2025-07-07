using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class TurnUIManager : MonoBehaviour
{
    public static TurnUIManager Instance;

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

    public bool isAnimatingReset = false;
    public bool IsAnimatingReset => isAnimatingReset;

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
                var character = characters[i];
                hud.SetData(character.icon, character.speed + character.initiativeRoll);
                currentTurnHUDs.Add(hud);

                float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

                hud.transform.localPosition = pos;
                hud.transform.rotation = Quaternion.identity;
                hud.SetOriginalPosition(pos);
            }
        }
    }

    public void HighlightCurrentTurn(int index)
    {
        if (isAnimatingReset)
            return;

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

        float angleDifference = Mathf.DeltaAngle(startAngle, targetAngle);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float angle = startAngle + angleDifference * t;

            turnHUDContainer.localEulerAngles = new Vector3(0, 0, angle);

            foreach (var hud in currentTurnHUDs)
                hud.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            yield return null;
        }

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

    // 🔁 Animación general que se llama desde RoundManager
    public IEnumerator AnimateResetAndRebuild(List<Character> characters)
    {
        isAnimatingReset = true;

        yield return MoveIconsToCenter();
        yield return RebuildHUDs(characters);
        yield return AnimateIconsToCircle();

        isAnimatingReset = false;
    }

    // Etapa 1: mover íconos al centro
    private IEnumerator MoveIconsToCenter(float duration = 0.5f)
    {
        if (currentTurnHUDs.Count == 0)
            yield break;

        float elapsed = 0f;
        List<Vector3> startPositions = new();

        foreach (var hud in currentTurnHUDs)
            startPositions.Add(hud.transform.localPosition);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            for (int i = 0; i < currentTurnHUDs.Count; i++)
            {
                if (currentTurnHUDs[i] != null)
                {
                    currentTurnHUDs[i].transform.localPosition = Vector3.Lerp(startPositions[i], Vector3.zero, t);
                    currentTurnHUDs[i].transform.rotation = Quaternion.identity;
                }
            }

            yield return null;
        }
    }

    // Etapa 2: destruir e instanciar íconos con el nuevo orden
    private IEnumerator RebuildHUDs(List<Character> characters)
    {
        foreach (Transform child in turnHUDContainer)
            Destroy(child.gameObject);

        currentTurnHUDs.Clear();

        yield return null; // Esperamos un frame

        // Instanciamos, pero se acomodan con UpdateTurnHUDs
        // más tarde los moveremos desde el centro
        UpdateTurnHUDs(characters);

        // Dejamos los íconos en el centro para animarlos luego
        foreach (var hud in currentTurnHUDs)
        {
            if (hud != null)
            {
                hud.transform.localPosition = Vector3.zero;
                hud.transform.rotation = Quaternion.identity;
            }
        }
    }

    // Etapa 3: animación desde el centro al círculo
    private IEnumerator AnimateIconsToCircle(float duration = 0.5f)
    {
        float elapsed = 0f;
        int count = currentTurnHUDs.Count;
        float angleStep = totalAngle / count;
        float startAngle = (360f - totalAngle) / 2f;

        List<Vector3> finalPositions = new();

        for (int i = 0; i < count; i++)
        {
            float angle = (startAngle + angleStep * i) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            finalPositions.Add(pos);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            for (int i = 0; i < count; i++)
            {
                var hud = currentTurnHUDs[i];
                if (hud != null)
                {
                    hud.transform.localPosition = Vector3.Lerp(Vector3.zero, finalPositions[i], t);
                    hud.transform.rotation = Quaternion.identity;
                }
            }

            yield return null;
        }

        for (int i = 0; i < count; i++)
        {
            var hud = currentTurnHUDs[i];
            if (hud != null)
            {
                hud.transform.localPosition = finalPositions[i];
                hud.transform.rotation = Quaternion.identity;
                hud.SetOriginalPosition(finalPositions[i]);
            }
        }
    }
}
