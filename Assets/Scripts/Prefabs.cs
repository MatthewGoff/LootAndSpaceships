using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static Prefabs Instance;

    public GameObject Bullet;
    public GameObject Coin;
    public GameObject Crate;
    public GameObject EMP;
    public GameObject Enemy;
    public GameObject Explosion;
    public GameObject ExpMorsel;
    public GameObject FDN;
    public GameObject FDNText;
    public GameObject FuelRod;
    public GameObject LevelUpBlast;
    public GameObject ParallaxLayer;
    public GameObject Player;
    public GameObject RadarArrow;
    public GameObject RadarPip;
    public GameObject Rocket;
    public GameObject Scrap;

    private void Awake()
    {
        Instance = this;
    }
}
