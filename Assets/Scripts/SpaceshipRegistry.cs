using System.Collections.Generic;

/// <summary>
/// Administer unique integer identifiers (UID) to spaceships
/// </summary>
public class SpaceshipRegistry
{
    public static readonly int NULL_UID = -1;
    public static SpaceshipRegistry Instance;
    public bool[] UIDInUse;
    public Dictionary<int, Spaceship> Spaceships;

    public static void Initialize()
    {
        Instance = new SpaceshipRegistry();
    }

    private SpaceshipRegistry()
    {
        UIDInUse = new bool[10];
        Spaceships = new Dictionary<int, Spaceship>();
    }

    public int RegisterSpaceship(Spaceship spaceship)
    {
        int newUID = GetUnusedUID();
        UIDInUse[newUID] = true;
        Spaceships.Add(newUID, spaceship);
        return newUID;
    }

    /// <summary>
    /// Free up an identifier when a spaceship dies or otherwise exits the
    /// scene.
    /// </summary>
    /// <param name="uid"></param>
    public void UnregisterSpaceship(int uid)
    {
        UIDInUse[uid] = false;
        Spaceships.Remove(uid);
    }

    private int GetUnusedUID()
    {
        for (int i = 0; i < UIDInUse.Length; i++)
        {
            if (!UIDInUse[i])
            {
                return i;
            }
        }
        ExpandUIDArray();
        return GetUnusedUID();
    }

    private void ExpandUIDArray()
    {
        bool[] newUIDINUse = new bool[UIDInUse.Length * 2];
        for (int i = 0; i < UIDInUse.Length; i++)
        {
            newUIDINUse[i] = UIDInUse[i];
        }
        UIDInUse = newUIDINUse;
    }
}
