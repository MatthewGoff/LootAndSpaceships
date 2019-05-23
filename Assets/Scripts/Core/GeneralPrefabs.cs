using UnityEngine;

public class GeneralPrefabs : MonoBehaviour
{
    public static GeneralPrefabs Instance;

    public GameObject Bullet;
    public GameObject Coin;
    public GameObject Crate;
    public GameObject EMP;
    public GameObject Explosion;
    public GameObject ExpMorsel;
    public GameObject FDN;
    public GameObject FDNText;
    public GameObject Flamethrower;
    public GameObject FlamethrowerParticle;
    public GameObject FuelRod;
    public GameObject Harpoon;
    public GameObject HarpoonHook;
    public GameObject HarpoonHorizontalLink;
    public GameObject HarpoonVerticalLink;
    public GameObject LaserBeam;
    public GameObject LaserTip;
    public GameObject LevelUpBlast;
    public GameObject Mine;
    public GameObject ParallaxLayer;
    public GameObject RadarArrow;
    public GameObject RadarPip;
    public GameObject Rocket;
    public GameObject Scrap;

    private void Awake()
    {
        Instance = this;
    }
}
