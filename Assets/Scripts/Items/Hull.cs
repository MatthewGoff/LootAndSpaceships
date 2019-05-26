using UnityEngine;

public class Hull : Item
{
    public HullModel Model;
    public float Size;
    public float Mass;
    public float HullSpace;
    public float MaximumHitPoints;
    public float CurrentHitPoints;
    public TargetingType TargetingType;

    public Hull
    (
        HullModel model,
        float size,
        float massMultiplier,
        float hullSpaceMultiplier,
        float hitPoints,
        TargetingType targetingType
    ) : base(0, 0, ItemClass.Hull, ItemType.Hull)
    {
        Model = model;
        Size = size;
        Mass = massMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(Size, 3);
        HullSpace = hullSpaceMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(Size, 3);
        MaximumHitPoints = hitPoints;
        CurrentHitPoints = MaximumHitPoints;
        TargetingType = targetingType;
    }

    public Hull(HullParameters parameters) : base(0, 0, ItemClass.Hull, ItemType.Hull)
    {
        Model = parameters.Model;
        Size = parameters.Size;
        Mass = parameters.MassMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(Size, 3);
        HullSpace = parameters.HullSpaceMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(Size, 3);
        MaximumHitPoints = parameters.HitPoints;
        CurrentHitPoints = MaximumHitPoints;
        TargetingType = parameters.TargetingType;
    }

    public static Hull CreateRandomHull(int level)
    {
        return new Hull(0, 0, 0, 0, 0, TargetingType.Bound);
    }
}
