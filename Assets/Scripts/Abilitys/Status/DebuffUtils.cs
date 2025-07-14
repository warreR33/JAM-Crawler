public static class DebuffUtils
{
    public static DebuffCategory GetCategory(DebuffType type)
    {
        return type switch
        {
            DebuffType.WeakBurn or DebuffType.StrongBurn             => DebuffCategory.Burn,
            DebuffType.WeakDizzy or DebuffType.StrongDizzy           => DebuffCategory.Dizzy,
            DebuffType.WeakWeaken or DebuffType.StrongWeaken         => DebuffCategory.Weaken,
            DebuffType.WeakBleeding or DebuffType.StrongBleeding     => DebuffCategory.Bleeding,
            _ => throw new System.Exception("Unknown DebuffType")
        };
    }
}