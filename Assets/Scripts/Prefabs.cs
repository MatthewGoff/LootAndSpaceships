using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static Prefabs Instance;
    public GameObject Bullet;
    public GameObject EMP;
    public GameObject Enemy;
    public GameObject Explosion;
    public GameObject EXPMorsel;
    public GameObject FDN;
    public GameObject FDNText;
    public GameObject ParallaxLayer;
    public GameObject Player;
    public GameObject RadarArrow;
    public GameObject RadarPip;
    public GameObject Rocket;

    private void Awake()
    {
        Instance = this;
    }
}
