public static class BuffUtils
{
    public static BuffCategory GetCategory(BuffType type)
    {
        return type switch
        {
            BuffType.WeakRegeneration or BuffType.StrongRegeneration => BuffCategory.Regeneration,
            BuffType.WeakStrength or BuffType.StrongStrength         => BuffCategory.Strength,
            BuffType.WeakSpeedUp or BuffType.StrongSpeedUp           => BuffCategory.SpeedUp,
            BuffType.WeakHardened or BuffType.StrongHardened         => BuffCategory.Hardened,
            _ => throw new System.Exception("Unknown BuffType")
        };
    }
}