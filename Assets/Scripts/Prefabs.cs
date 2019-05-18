using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static Prefabs Instance;

    public GameObject Alpha1;
    public GameObject Alpha1Omni;
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
    public GameObject ParallaxLayer;
    public GameObject RadarArrow;
    public GameObject RadarPip;
    public GameObject Rocket;
    public GameObject Scrap;
    public GameObject Turret;

    private void Awake()
    {
        Instance = this;
    }
}
