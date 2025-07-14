using UnityEngine;

public class StatusEffectIconDatabase : MonoBehaviour
{
    private static Sprite[] cachedSprites;

    private static void LoadSprites()
    {
        if (cachedSprites == null)
            cachedSprites = Resources.LoadAll<Sprite>("Icons/status_icons");
    }

    public static Sprite GetDebuffIcon(DebuffType type)
    {
        LoadSprites();

        string spriteName = type switch
        {
            DebuffType.WeakBurn        => "burn_weak",
            DebuffType.StrongBurn      => "burn_strong",
            DebuffType.WeakDizzy       => "dizzy_weak",
            DebuffType.StrongDizzy     => "dizzy_strong",
            DebuffType.WeakWeaken      => "weaken_weak",
            DebuffType.StrongWeaken    => "weaken_strong",
            DebuffType.WeakBleeding    => "bleed_weak",
            DebuffType.StrongBleeding  => "bleed_strong",
            _ => null
        };

        return FindSpriteByName(spriteName);
    }

    public static Sprite GetBuffIcon(BuffType type)
    {
        LoadSprites();

        string spriteName = type switch
        {
            BuffType.WeakRegeneration     => "regen_weak",
            BuffType.StrongRegeneration   => "regen_strong",
            BuffType.WeakStrength         => "strength_weak",
            BuffType.StrongStrength       => "strength_strong",
            BuffType.WeakSpeedUp          => "speed_weak",
            BuffType.StrongSpeedUp        => "speed_strong",
            BuffType.WeakHardened         => "hardened_weak",
            BuffType.StrongHardened       => "hardened_strong",
            _ => null
        };

        return FindSpriteByName(spriteName);
    }

    private static Sprite FindSpriteByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        foreach (var sprite in cachedSprites)
        {
            if (sprite.name == name)
                return sprite;
        }

        Debug.LogWarning($"Sprite '{name}' no encontrado en 'status_icons'.");
        return null;
    }
}
