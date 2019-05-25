using UnityEngine;

public class BottomHUDController : MonoBehaviour
{
    public static BottomHUDController Instance;

    public GameObject HealthBar;
    public GameObject ShieldBar;
    public GameObject EnergyBar;
    public GameObject FuelBar;

    public void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance.PlayerAlive)
        {
            RadarProfile profile = GameManager.Instance.PlayerController.GetRadarProfile();
            HealthBar.transform.localScale = new Vector2(1, profile.CurrentHealth / profile.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, profile.CurrentShield / profile.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, profile.CurrentEnergy / profile.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, profile.CurrentFuel / profile.MaxFuel);
        }
        else
        {
            HealthBar.transform.localScale = new Vector2(1, 0);
            ShieldBar.transform.localScale = new Vector2(1, 0);
            EnergyBar.transform.localScale = new Vector2(1, 0);
            FuelBar.transform.localScale = new Vector2(1, 0);
        }
    }
}
