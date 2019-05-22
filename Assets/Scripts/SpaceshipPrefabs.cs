using UnityEngine;

public class SpaceshipPrefabs : MonoBehaviour
{
    public static SpaceshipPrefabs Instance;

    public GameObject Alpha1;
    public GameObject Alpha1Omni;
    public GameObject Alpha2;
    public GameObject Alpha3;
    public GameObject Alpha4;
    public GameObject Beta1;
    public GameObject Delta1;
    public GameObject Eta1;
    public GameObject Gamma1;
    public GameObject Iota1;
    public GameObject Nu1;
    public GameObject Omicron1;
    public GameObject Phi1;
    public GameObject Pi1;
    public GameObject Pi2;
    public GameObject Sigma1;
    public GameObject Tau1;
    public GameObject Theta1;
    public GameObject Turret;
    public GameObject Upsilon1;
    public GameObject Xi1;
    public GameObject Zeta1;

    private void Awake()
    {
        Instance = this;
    }
}
