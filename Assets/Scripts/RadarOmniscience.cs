using System.Collections.Generic;

/**
 * The Radar Omniscience is a singleton class which is responsible for
 * administering visibility of information between spaceships. All spaceships
 * are responsible to update the radar omniscience with a RadarProfile once per
 * fixed update. Spaceships can ping the radar omniscience with a
 * clearance level to recieve a list of redacted RadarProfiles for all the
 * spaceships in the scene.
 */
public class RadarOmniscience
{
    public static RadarOmniscience Instance;
    private RadarProfile[] RadarProfiles;
    private bool[] RadarIdentifierInUse;

    public static void Initialize()
    {
        Instance = new RadarOmniscience();
        Instance.RadarProfiles = new RadarProfile[10];
        Instance.RadarIdentifierInUse = new bool[10];
    }

    /**
     * Register a new radar entity with the radar omnicience and return
     * a unique "radar identifier".
     */
    public int RegisterNewRadarEntity()
    {
        int newRadarIdentifier = GetUnusedIdentifier();
        RadarIdentifierInUse[newRadarIdentifier] = true;
        return newRadarIdentifier;
    }

    /**
     * Inform the radar omniscience that a registered radar entity is no longer
     * present in the scene.
     */
    public void RemoveRegisteredRadarEntity(int radarIdentifier)
    {
        RadarProfiles[radarIdentifier] = null;
        RadarIdentifierInUse[radarIdentifier] = false;
    }

    public void SubmitRadarProfile(int radarIdentifier, RadarProfile profile)
    {
        RadarProfiles[radarIdentifier] = profile;
    }

    private int GetUnusedIdentifier()
    {
        for (int i = 0; i < RadarIdentifierInUse.Length; i++)
        {
            if (!RadarIdentifierInUse[i])
            {
                return i;
            }
        }
        ExpandRadarProfiles();
        return GetUnusedIdentifier();
    }

    private void ExpandRadarProfiles()
    {
        RadarProfile[] newRadarProfiles = new RadarProfile[RadarProfiles.Length * 2];
        bool[] newRadarIdentifierInUse = new bool[RadarIdentifierInUse.Length * 2];
        for (int i = 0; i < RadarProfiles.Length; i++)
        {
            newRadarProfiles[i] = RadarProfiles[i];
            newRadarIdentifierInUse[i] = RadarIdentifierInUse[i];
        }
        RadarProfiles = newRadarProfiles;
        RadarIdentifierInUse = newRadarIdentifierInUse;
    }

    public LinkedList<RadarProfile> PingRadar(int radarIdentifier)
    {
        LinkedList<RadarProfile> profiles = new LinkedList<RadarProfile>();
        for (int i = 0; i < RadarProfiles.Length; i++)
        {
            if (i != radarIdentifier && RadarIdentifierInUse[i])
            {
                profiles.AddFirst(RadarProfiles[i]);
            }
        }
        return profiles;
    }
}
