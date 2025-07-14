using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI durationText;

    private int currentDuration;

    public void Init(Sprite sprite, int duration)
    {
        iconImage.sprite = sprite;
        SetDuration(duration);
    }

    public void SetDuration(int duration)
    {
        currentDuration = duration;
        durationText.text = duration.ToString();
    }

    public int GetDuration() => currentDuration;
}
