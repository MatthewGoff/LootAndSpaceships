using UnityEngine;

/// <summary>
/// Class representing a cooldown
/// </summary>
public class Cooldown
{
    /// <summary>
    /// The time which must elapse before the cooldown is ready again
    /// </summary>
    private float Duration;
    /// <summary>
    /// The last time the cooldown was used in seconds since Unity started
    /// </summary>
    private float LastUseTime;

    /// <summary>
    /// Create a new cooldown
    /// </summary>
    /// <param name="duration">
    /// The duration of this cooldown
    /// </param>
    public Cooldown(float duration)
    {
        Duration = duration;
        LastUseTime = Time.time - Duration;
    }

    /// <summary>
    /// Attempt to use this cooldown
    /// </summary>
    /// <returns>
    /// True if the cooldown was available
    /// </returns>
    public bool Use()
    {
        float currentTime = Time.time;
        if (currentTime - LastUseTime > Duration)
        {
            LastUseTime = currentTime;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Change the duration of this cooldown without alterinig its last use time
    /// </summary>
    /// <param name="duration">
    /// The new duration for this cooldown
    /// </param>
    public void Modify(float duration)
    {
        Duration = duration;
    }
}