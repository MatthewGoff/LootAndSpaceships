using System.Collections;
using UnityEngine;

public class Spaceship : Vehicle
{
    private float BurnDuration;
    private GameObject FireEffect;
    private float BurnEndTime;
    private bool Burning;
    private FDNCanvasController FDNCanvasController;
    private float MaxShield;
    private float CurrentShield;
    private float ShieldRegen;
    private float MaxHP;
    private float CurrentHP;
    private float HPRegen;
    private float MaxEnergy;
    private float CurrentEnergy;
    private float EnergyRegen;
    private float MaxFuel;
    private float CurrentFuel;
    private float FuelUsage;
    private float MaxHullSpace;
    private float CurrentHullSpace;
    private string Name;

    protected void Initialize(RadarType radarType,
        int team,
        float thrustForce,
        float turnRate,
        float maximumSpeed,
        float mass,
        float burnDuration,
        float maxShield,
        float shieldRegen,
        float maxHP,
        float hpRegen,
        float maxEnergy,
        float energyRegen,
        float maxFuel,
        float fuelUsage,
        float maxHullSpace,
        string name,
        GameObject fireEffect
        )
    {
        base.Initialize(radarType, team, thrustForce, turnRate, maximumSpeed, mass);
        GameObject canvas = Instantiate(Prefabs.FDNCanvas, new Vector3(0f, 0f, 0f), Quaternion.identity);
        canvas.transform.SetParent(transform);
        FDNCanvasController = canvas.GetComponent<FDNCanvasController>();
        FDNCanvasController.Subject = gameObject;
        BurnDuration = burnDuration;
        MaxShield = maxShield;
        CurrentShield = MaxShield;
        ShieldRegen = shieldRegen;
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        HPRegen = hpRegen;
        MaxEnergy = maxEnergy;
        CurrentEnergy = MaxEnergy;
        EnergyRegen = energyRegen;
        MaxFuel = maxFuel;
        CurrentFuel = MaxFuel;
        FuelUsage = fuelUsage;
        MaxHullSpace = maxHullSpace;
        CurrentHullSpace = MaxHullSpace;
        Name = name;
        FireEffect = fireEffect;
    }

    public override void RecieveHit(int damage, DamageType damageType)
    {
        FDNCanvasController.Display(damage, damage / 100f);
        if (damageType == DamageType.Explosion)
        {
            BurnEndTime = Time.time + BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }
    }

    private IEnumerator Burn()
    {
        Burning = true;
        FireEffect.SetActive(true);

        while (Time.time < BurnEndTime)
        {
            RecieveHit(Mathf.RoundToInt(Random.Range(0f, 10f)), DamageType.Fire);
            yield return new WaitForSeconds(0.1f);
        }

        Burning = false;
        FireEffect.SetActive(false);
    }

    public override Vector2 GetPosition()
    {
        return RB2D.position;
    }
}
