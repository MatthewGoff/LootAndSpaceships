using UnityEngine;

public static class Prefabs
{
    public static GameObject Bullet;
    public static GameObject EMP;
    public static GameObject Enemy;
    public static GameObject Explosion;
    public static GameObject FDN;
    public static GameObject FDNCanvas;
    public static GameObject ParallaxLayer;
    public static GameObject Player;
    public static GameObject RadarArrow;
    public static GameObject RadarPip;
    public static GameObject Rocket;

	public static void LoadPrefabs()
    {
        Bullet = (GameObject)Resources.Load("Prefabs/Bullet");
        EMP = (GameObject)Resources.Load("Prefabs/EMP");
        Enemy = (GameObject)Resources.Load("Prefabs/Enemy");
        Explosion = (GameObject)Resources.Load("Prefabs/Explosion");
        FDN = (GameObject)Resources.Load("Prefabs/FDN");
        FDNCanvas = (GameObject)Resources.Load("Prefabs/FDNCanvas");
        ParallaxLayer = (GameObject)Resources.Load("Prefabs/ParallaxLayer");
        Player = (GameObject)Resources.Load("Prefabs/Player");
        RadarArrow = (GameObject)Resources.Load("Prefabs/RadarArrow");
        RadarPip = (GameObject)Resources.Load("Prefabs/RadarPip");
        Rocket = (GameObject)Resources.Load("Prefabs/Rocket");
    }
}
