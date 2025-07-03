using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnHUD : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text speedText;

    private Vector3 originalPosition;
    public float highlightOffset = -20f;
    public float moveSpeed = 5f;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void SetData(Sprite icon, int totalSpeed)
    {
        if (iconImage != null)
            iconImage.sprite = icon;

        if (speedText != null)
            speedText.text = totalSpeed.ToString();
    }


    public void HighlightTurn()
    {
        StopAllCoroutines();
        Vector3 direction = (transform.localPosition).normalized;
        Vector3 offset = direction * 40f;
        StartCoroutine(MoveToPosition(originalPosition + offset));
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
    }

    public void SetOriginalPosition(Vector3 pos)
    {
        originalPosition = pos;
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.localPosition = target;
    }


}
