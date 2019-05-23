using System.Collections.Generic;

/// <summary>
/// The Radar Omniscience is a singleton class responsible for
/// administering visibility of information between spaceships. All spaceships
/// are responsible to update the radar omniscience with a RadarProfile once
/// per fixed update. Spaceships can ping the radar omniscience with a
/// clearance level to recieve a list of redacted RadarProfiles for all the
/// spaceships in the scene.
/// </summary>
public class RadarOmniscience
{
    public static RadarOmniscience Instance;
    private Dictionary<int, RadarProfile> RadarProfiles;

    public static void Initialize()
    {
        Instance = new RadarOmniscience();
    }

    private RadarOmniscience()
    {
        RadarProfiles = new Dictionary<int, RadarProfile>();
    }

    /// <summary>
    /// Register a new radar entity with the radar omnicience
    /// </summary>
    /// <param name="uid"></param>
    public void RegisterNewRadarEntity(int uid)
    {
        RadarProfiles.Add(uid, null);
    }

    /// <summary>
    /// Inform the radar omniscience that a registered radar entity is no
    /// longer present in the scene.
    /// </summary>
    /// <param name="uid"></param>
    public void UnregisterRadarEntity(int uid)
    {
        RadarProfiles.Remove(uid);
    }

    public void SubmitRadarProfile(int uid, RadarProfile profile)
    {
        RadarProfiles[uid] = profile;
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
}
