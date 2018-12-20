using UnityEngine;

public abstract class RadarTarget : MonoBehaviour, ITargetable
{
    public RadarType RadarType { get; protected set;}

    protected void Initialize(RadarType radarType)
    {
        RadarType = radarType;
    }

    public abstract Vector2 GetPosition();
}
