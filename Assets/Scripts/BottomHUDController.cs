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
            DirectedPlayerController subject = GameManager.Instance.PlayerController;
            HealthBar.transform.localScale = new Vector2(1, subject.CurrentHealth / subject.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, subject.CurrentShield / subject.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, subject.CurrentEnergy / subject.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, subject.CurrentFuel / subject.MaxFuel);
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
