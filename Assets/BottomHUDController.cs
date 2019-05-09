using UnityEngine;

public class BottomHUDController : MonoBehaviour
{
    public static BottomHUDController Instance;

    public Spaceship Subject;
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
        if (Subject != null)
        {
            HealthBar.transform.localScale = new Vector2(1, Subject.CurrentHealth / Subject.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, Subject.CurrentShield / Subject.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, Subject.CurrentEnergy / Subject.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, Subject.CurrentFuel / Subject.MaxFuel);
        }
    }
}
