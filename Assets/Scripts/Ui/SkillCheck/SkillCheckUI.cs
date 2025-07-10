using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SkillCheckResult {Normal, Success }

public class SkillCheckUI : MonoBehaviour
{
    public RectTransform backgroundBar;
    public RectTransform successZone;
    public RectTransform movingCursor;

    public float moveDuration = 2f;
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool hasPressedKey = false;

    private System.Action<SkillCheckResult> onFinished; // NUEVO

    public void StartSkillCheck(System.Action<SkillCheckResult> callback)
    {
        onFinished = callback;
        gameObject.SetActive(true);
        ResetSkillCheck();
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        float halfBarWidth = backgroundBar.rect.width / 2f;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);
        float cursorX = Mathf.Lerp(-halfBarWidth, halfBarWidth, t);
        movingCursor.anchoredPosition = new Vector2(cursorX, movingCursor.anchoredPosition.y);

        if (Input.GetKeyDown(KeyCode.Space) && !hasPressedKey)
        {
            hasPressedKey = true;
            isRunning = false;
            CheckSuccess();
        }

        if (t >= 1f && !hasPressedKey)
        {
            isRunning = false;
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        float cursorX = movingCursor.anchoredPosition.x;
        float zoneMin = successZone.anchoredPosition.x - successZone.rect.width / 2;
        float zoneMax = successZone.anchoredPosition.x + successZone.rect.width / 2;

        if (cursorX >= zoneMin && cursorX <= zoneMax)
        {
            Finish(SkillCheckResult.Success);
        }
        else
        {
            Finish(SkillCheckResult.Normal);
        }
    }

    void Finish(SkillCheckResult result)
    {
        RectTransform target = result == SkillCheckResult.Success ? successZone : backgroundBar;
        StartCoroutine(ShakeAndHide(target, result));
    }

    IEnumerator ShakeAndHide(RectTransform target, SkillCheckResult result)
    {
        Vector3 originalPos = target.anchoredPosition;
        float duration = 1f;
        float time = 0f;
        float magnitude = 3f;

        while (time < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            target.anchoredPosition = originalPos + new Vector3(x, y, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = originalPos;

        // Ocultar y devolver resultado
        onFinished?.Invoke(result);
        Destroy(gameObject); // si es un prefab, lo destruimos después del uso
    }

    void ResetSkillCheck()
    {
        elapsedTime = 0f;
        hasPressedKey = false;

        float cursorStartX = -backgroundBar.rect.width / 2f;
        movingCursor.anchoredPosition = new Vector2(cursorStartX, movingCursor.anchoredPosition.y);

        float zoneWidth = successZone.rect.width;
        float halfBarWidth = backgroundBar.rect.width / 2f;
        float minX = -halfBarWidth + zoneWidth / 2f;
        float maxX = halfBarWidth - zoneWidth / 2f;

        float randomX = Random.Range(minX, maxX);
        successZone.anchoredPosition = new Vector2(randomX, successZone.anchoredPosition.y);
    }
}
