using System.Collections.Generic;

/// <summary>
/// The Omniscience is a singleton class responsible for
/// administering visibility of information between spaceships. All spaceships
/// are responsible to update the omniscience with a RadarProfile once
/// per fixed update. Spaceships can ping the radar omniscience with a
/// clearance level to recieve a list of redacted RadarProfiles for all the
/// spaceships in the scene.
/// </summary>
public class Omniscience
{
    public static readonly int NULL_UID = -1;

    public static Omniscience Instance;

    private bool[] UIDInUse;
    private Dictionary<int, Spaceship> Spaceships;
    private Dictionary<int, RadarProfile> RadarProfiles;

    public static void Initialize()
    {
        Instance = new Omniscience();
    }

    private Omniscience()
    {
        RadarProfiles = new Dictionary<int, RadarProfile>();
        UIDInUse = new bool[10];
        Spaceships = new Dictionary<int, Spaceship>();
    }

    /// <summary>
    /// Register a new entity with the omnicience
    /// </summary>
    /// <param name="uid"></param>
    public int RegisterNewEntity(Spaceship spaceship)
    {
        int newUID = GetUnusedUID();
        UIDInUse[newUID] = true;
        Spaceships.Add(newUID, spaceship);
        RadarProfiles.Add(newUID, null);
        return newUID;
    }

    /// <summary>
    /// Inform the omniscience that a registered entity is no
    /// longer present in the scene.
    /// </summary>
    /// <param name="uid"></param>
    public void UnregisterEntity(int uid)
    {
        RadarProfiles.Remove(uid);
        Spaceships.Remove(uid);
        UIDInUse[uid] = false;
    }

    public void SubmitRadarProfile(int uid, RadarProfile profile)
    {
        RadarProfiles[uid] = profile;
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

    public int CountTeamMembers(int team)
    {
        int count = 0;
        foreach (int key in Spaceships.Keys)
        {
            if (Spaceships[key].Team == team)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Retrieve all radar profiles from the current scene. Excluding the one
    /// belonging to the provided UID.
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public Dictionary<int, RadarProfile> PingRadar(int uid)
    {
        Dictionary<int, RadarProfile> profiles = new Dictionary<int, RadarProfile>();
        foreach (int key in RadarProfiles.Keys)
        {
            if (key != uid && RadarProfiles[key] != null)
            {
                profiles.Add(key, RadarProfiles[key]);
            }
        }
        return profiles;
    }

    /// <summary>
    /// Retrieve all radar profiles from the current scene
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, RadarProfile> PingRadar()
    {
        Dictionary<int, RadarProfile> profiles = new Dictionary<int, RadarProfile>();
        foreach (int key in RadarProfiles.Keys)
        {
            if (RadarProfiles[key] != null)
            {
                profiles.Add(key, RadarProfiles[key]);
            }
        }
        return profiles;
    }

    public Spaceship GetSpaceship(int UID)
    {
        if (Spaceships.ContainsKey(UID))
        {
            return Spaceships[UID];
        }
        else
        {
            return null;
        }
    }
}
