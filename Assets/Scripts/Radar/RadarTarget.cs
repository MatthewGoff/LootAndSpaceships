using UnityEngine;

public abstract class RadarTarget : MonoBehaviour, ITargetable
{
    public RadarType RadarType { get; protected set;}

    public abstract Vector2 GetPosition();
}
