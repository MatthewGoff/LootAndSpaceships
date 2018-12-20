using UnityEngine;

public class EnemyController : Spaceship
{
    public void Initialize(string name)
    {
        base.Initialize(
            radarType: RadarType.Enemy,
            team: 1,
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f,
            burnDuration: 3f,
            maxShield: 100f,
            shieldRegen: 1f,
            maxHP: 100f,
            hpRegen: 1f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 523.6f,
            name: name);
        RadarController.Instance.AddToRadar(this);
    }

    private void OnMouseDown()
    {
        if (!GameManager.MouseOverUI())
        {
            GameManager.Instance.ChangeTarget(this);
        }
    }
}
