using UnityEngine;

public class StaticTarget : ITargetable
{
    private readonly Vector2 Position;

	public StaticTarget(Vector2 position)
    {
        Position = position;
    }

    public Vector2 GetPosition()
    {
        return Position;
    }
}
