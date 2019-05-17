﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Spaceship
{
    public GameObject AutopilotTargetEffect;
    public GameObject ExhaustEffect;
    public int Experience;
    public int Level;

    private Autopilot Autopilot;
    private bool UsingAutopilot;
    private int AttackType;

    public void Initialize(string name)
    {
        VehicleController vehicleController = new DirectedVehicleController(
            rb2d: GetComponent<Rigidbody2D>(),
            thrustForce: 10f,
            turnRate: 300f,
            maximumSpeed: 30f,
            mass: 1f
            );
        base.Initialize(
            vehicleController: vehicleController,
            team: 0,
            burnDuration: 3f,
            maxShield: 250f,
            shieldRegen: 20f,
            shieldEnergy: 0.1f,
            maxHP: 500f,
            hpRegen: 5f,
            maxEnergy: 10f,
            energyRegen: 1f,
            maxFuel: 510,
            fuelUsage: 0.85f,
            maxHullSpace: 523.6f,
            name: name,
            attackEnergy: 1.5f,
            thrustEnergy: 1.5f,
            lifeSupportEnergy: 0.1f,
            lifeSupportDegen: 10f);

        Experience = 0;
        Level = 0;
        AttackType = 1;
        UsingAutopilot = false;
        Autopilot = new FastAutopilotDirected(VehicleController);
        ShowFDN = false;
        ExhaustEffect.SetActive(true);
    }

    private void Update()
    {
        ZeroInput();

        GetVehicleController().TurnInput = -Input.GetAxis("Horizontal");
        GetVehicleController().ThrustInput = Input.GetKey(KeyCode.W);
        GetVehicleController().BreakInput = Input.GetKey(KeyCode.S);

        if (GetVehicleController().TurnInput != 0f
            || GetVehicleController().ThrustInput
            || GetVehicleController().BreakInput)
        {
            DismissAutopilot();
        }

        if (Input.GetMouseButton(1) && !GameManager.MouseOverUI())
        {
            Vector2 target = MasterCameraController.GetMousePosition();
            Autopilot.SetTarget(target, AutopilotBehaviour.Seek);
            UsingAutopilot = true;
            AutopilotTargetEffect.SetActive(true);
            AutopilotTargetEffect.transform.position = MasterCameraController.GetMousePosition();
        }

        if (UsingAutopilot)
        {
            if (Autopilot.Update())
            {
                DismissAutopilot();
            }
        }

        if (GetVehicleController().ThrustInput)
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Enable();
        }
        else
        {
            ExhaustEffect.GetComponent<ExhaustEffectController>().Disable();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AttackType = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AttackType = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AttackType = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AttackType = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AttackType = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AttackType = 6;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (AttackType == 1)
            {
                FireBullet = true;
            }
            else if (AttackType == 2)
            {
                FireRocket = true;
            }
            else if (AttackType == 3)
            {
                FireEMP = true;
            }
            else if (AttackType == 4)
            {
                FireHarpoon = true;
            }
            else if (AttackType == 5)
            {
                FireFlamethrower = true;
            }
            else if (AttackType == 6)
            {
                FireLaser = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Die();
        }

        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleTarget();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DropTarget();
        }
        if (!RadarOmniscience.Instance.PingRadar().ContainsKey(TargetUID))
        {
            DropTarget();
        }
    }

    public void CycleTarget()
    {
        List<int> uids = new List<int>(GetRadarReading().Keys);
        if (uids.Count > 0)
        {
            uids.Sort();
            foreach (int uid in uids)
            {
                if (uid > TargetUID)
                {
                    SelectTarget(uid);
                    return;
                }
            }
            SelectTarget(uids[0]);
        }
    }

    private void DismissAutopilot()
    {
        UsingAutopilot = false;
        AutopilotTargetEffect.SetActive(false);
    }

    public override void TakeDamage(AttackManager attackManager, float damage, DamageType damageType)
    {
        base.TakeDamage(attackManager, damage, damageType);
    }

    protected override void Die()
    {
        GameManager.Instance.PlayerDeath();
        AutopilotTargetEffect.SetActive(false);
        base.Die();
    }

    public override void PickupExp(int quantity)
    {
        Experience += quantity;
        if (Experience >= Configuration.ExpForLevel(Level + 1))
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience = 0;
        GameManager.Instance.PlayerLevelUp(Level);
    }

    private DirectedVehicleController GetVehicleController()
    {
        return (DirectedVehicleController)VehicleController;
    }
}
