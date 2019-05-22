using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public GameObject PortraitCamera;
    public GameObject FireEffect;

    public string Name;

    public bool FireBullet;
    public bool FireRocket;
    public bool FireEMP;
    public bool FireHarpoon;
    public bool FireFlamethrower;
    public bool FireLaser;
    public bool FireMine;

    public float Credits;
    public float Scrap;
    public int Bullets;
    public int Rockets;
    public int Mines;
    public int Drones;
    public int Turrets;

    public bool HasTarget;
    public int TargetUID;
    private bool HasValidTarget
    {
        get
        {
            ValidateTarget();
            return HasTarget;
        }
    }

    public Vector2 Position
    {
        get
        {
            if (RB2D == null)
            {
                return transform.position;
            }
            else
            {
                return RB2D.position;
            }
        }
    }
    public Vector2 Velocity
    {
        get
        {
            return VehicleController.Velocity;
        }
    }

    protected bool ShowFDN;

    protected int UID;
    private float BurnDuration;
    private float BurnEndTime;
    private bool Burning;
    public float MaxShield;
    public float CurrentShield;
    private float ShieldRegen;
    private float ShieldEnergy;
    public float MaxHealth;
    public float CurrentHealth;
    private float HealthRegen;
    public float MaxEnergy;
    public float CurrentEnergy;
    private float EnergyRegen;
    public float MaxFuel;
    public float CurrentFuel;
    private float FuelUsage;
    public float MaxHullSpace;
    private Cooldown AttackCooldown;
    protected float AttackEnergy;
    protected float ThrustEnergy;
    protected float LifeSupportEnergy;
    protected float LifeSupportDegen;

    private Rigidbody2D RB2D;
    private HarpoonAttackManager Harpoon;
    private FlamethrowerAttackManager Flamethrower;
    private LaserAttackManager Laser;
    private List<AttackImmunityRecord> Immunities;
    protected VehicleController VehicleController;
    public int Team;
    public Autopilot Autopilot;
    private AI AI;
    public AttackType AttackType;
    public AttackMode AttackMode;
    private TargetingType TargetingType;
    protected bool Thrusting;
    private int NumberOfDrones;
    private int NumberOfTurrets;
    public Player Player;
    private int LootExperience;
    private int LootCredits;
    private int LootFuel;
    private int LootScrap;
    private int LootItems;

    public bool PlayerControlled
    {
        get
        {
            return AI == null;
        }
    }

    public void Initialize(Player player, Spaceship mother, SpaceshipParameters parameters)
    {
        ShowFDN = (Team != 0);
        NumberOfDrones = 0;
        NumberOfTurrets = 0;
        AttackType = AttackType.Bullet;
        AttackMode = AttackMode.Self;
        RB2D = GetComponent<Rigidbody2D>();
        Flamethrower = new FlamethrowerAttackManager(this, 10f);
        Laser = new LaserAttackManager(this, 10f);
        Immunities = new List<AttackImmunityRecord>();
        UID = SpaceshipRegistry.Instance.RegisterSpaceship(this);
        RadarOmniscience.Instance.RegisterNewRadarEntity(UID);

        if (parameters.VehicleType == VehicleType.Directed)
        {
            VehicleController = new DirectedVehicleController
            (
                rb2d: GetComponent<Rigidbody2D>(),
                thrustForce: parameters.ThrustForce,
                turnRate: parameters.TurnRate,
                maximumSpeed: parameters.MaximumSpeed,
                mass: parameters.MassMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(transform.localScale.x, 3)
            );
        }
        else if (parameters.VehicleType == VehicleType.Omnidirectional)
        {
            VehicleController = new OmnidirectionalVehicleController
            (
                rb2d: GetComponent<Rigidbody2D>(),
                thrustForce: parameters.ThrustForce,
                maximumSpeed: parameters.MaximumSpeed,
                mass: parameters.MassMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(transform.localScale.x, 3)
            );
        }
        else if (parameters.VehicleType == VehicleType.Stationary)
        {
            VehicleController = new StationaryVehicleController
            (
                rb2d: GetComponent<Rigidbody2D>(),
                thrustForce: parameters.ThrustForce,
                maximumSpeed: parameters.MaximumSpeed,
                mass: parameters.MassMultiplier * (4f / 3f) * Mathf.PI * Mathf.Pow(transform.localScale.x, 3)
            );
        }

        if (VehicleController.VehicleType == VehicleType.Directed)
        {
            Autopilot = new FastAutopilotDirected(VehicleController);
        }
        else if (VehicleController.VehicleType == VehicleType.Omnidirectional)
        {
            Autopilot = new FastAutopilotOmnidirectional(VehicleController);
        }
        else if (VehicleController.VehicleType == VehicleType.Stationary)
        {
            Autopilot = new StationaryAutopilot(VehicleController);
        }
        AI = AI.CreateAI(parameters.AIType, this, Autopilot, mother, parameters.AIParameters);

        Player = player;
        TargetingType = parameters.TargetingType;
        Team = parameters.Team;
        BurnDuration = parameters.BurnDuration;
        MaxShield = parameters.MaxShield;
        CurrentShield = MaxShield;
        ShieldRegen = parameters.ShieldRegen;
        ShieldEnergy = parameters.ShieldEnergy;
        MaxHealth = parameters.MaxHealth;
        CurrentHealth = MaxHealth;
        HealthRegen = parameters.HealthRegen;
        MaxEnergy = parameters.MaxEnergy;
        CurrentEnergy = MaxEnergy;
        EnergyRegen = parameters.EnergyRegen;
        MaxFuel = parameters.MaxFuel;
        CurrentFuel = MaxFuel;
        FuelUsage = parameters.FuelUsage;
        MaxHullSpace = parameters.HullSpaceMultiplier * (4f/3f) * Mathf.PI * Mathf.Pow(transform.localScale.x, 3);
        Name = parameters.Name;
        AttackEnergy = parameters.AttackEnergy;
        ThrustEnergy = parameters.ThrustEnergy;
        LifeSupportEnergy = parameters.LifeSupportEnergy;
        LifeSupportDegen = parameters.LifeSupportDegen;
        AttackCooldown = new Cooldown(parameters.AttackCooldown);
        LootExperience = parameters.LootExperience;
        LootCredits = parameters.LootCredits;
        LootFuel = parameters.LootFuel;
        LootScrap = parameters.LootScrap;
        LootItems = parameters.LootItems;

        Credits = 0f;
        Scrap = MaxHullSpace;
        Bullets = 0;
        Rockets = 0;
        Mines = 0;
        Drones = 0;
        Turrets = 0;

        ModelSpecificInitialization();
    }

    protected virtual void ModelSpecificInitialization()
    {

    }

    public virtual void TakeDamage(AttackManager attackManager, float damage, DamageType damageType)
    {
        if (attackManager != null)
        {
            if (ImmuneToDamage(attackManager))
            {
                return;
            }
            else if (attackManager.ImmunityDuration > 0)
            {
                Immunize(attackManager);
            }
            if (!PlayerControlled)
            {
                AI.AlertDamage(attackManager.Attacker);
            }
        }

        if (ShowFDN)
        {
            GameObject fdn = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FDN, VehicleController.Position, Quaternion.identity);
            fdn.GetComponent<FDNController>().Display(Mathf.RoundToInt(damage), damage / 100f);
        }

        if (damageType == DamageType.Explosion || damageType == DamageType.Fire)
        {
            BurnEndTime = Time.time + BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }

        float shieldDamage = Mathf.Min(CurrentShield, damage);
        CurrentShield -= shieldDamage;
        float remainingDamage = damage - shieldDamage;
        CurrentHealth -= remainingDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private IEnumerator Burn()
    {
        Burning = true;
        FireEffect.SetActive(true);

        while (Time.time < BurnEndTime)
        {
            TakeDamage(null, Mathf.RoundToInt(Random.Range(0f, 10f)), DamageType.Burning);
            yield return new WaitForSeconds(0.1f);
        }

        Burning = false;
        FireEffect.SetActive(false);
    }

    private void UpdateImmunities()
    {
        List<AttackImmunityRecord> expiredImmunities = new List<AttackImmunityRecord>();
        foreach (AttackImmunityRecord record in Immunities)
        {
            record.RemainingImmunityDuration -= Time.fixedDeltaTime;
            if (record.RemainingImmunityDuration <= 0f)
            {
                expiredImmunities.Add(record);
            }
        }
        foreach (AttackImmunityRecord record in expiredImmunities)
        {
            Immunities.Remove(record);
        }
    }

    private bool ImmuneToDamage(AttackManager attackManager)
    {
        foreach (AttackImmunityRecord record in Immunities)
        {
            if (record.AttackManager == attackManager)
            {
                return true;
            }
        }
        return false;
    }

    private void Immunize(AttackManager attackManager)
    {
        AttackImmunityRecord record = new AttackImmunityRecord
        {
            AttackManager = attackManager,
            RemainingImmunityDuration = attackManager.ImmunityDuration
        };
        Immunities.Add(record);
    }
    private void OnMouseDown()
    {
        if (!GameManager.MouseOverUI())
        {
            GameManager.Instance.SelectPlayerTarget(UID);
        }
    }

    private void Update()
    {
        ZeroAttackInputs();

        if (PlayerControlled)
        {
            PlayerUpdate();
        }
        else
        {
            AI.Update(RadarOmniscience.Instance.PingRadar(UID));
        }

        bool destinationReached = Autopilot.Update();
        if (destinationReached && PlayerControlled)
        {
            Autopilot.Standby();
        }

        ModelSpecificUpdate();
    }

    protected virtual void ModelSpecificUpdate()
    {

    }

    private void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerReload();
        }

        PlayerUpdateVehicleInputs();

        if (Input.GetMouseButton(1) && !GameManager.MouseOverUI())
        {
            Vector2 target = MasterCameraController.GetMousePosition();
            Autopilot.SetTarget(target, AutopilotBehaviour.Seek);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AttackType = AttackType.Bullet;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AttackType = AttackType.Rocket;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AttackType = AttackType.EMP;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AttackType = AttackType.Harpoon;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AttackType = AttackType.Flamethrower;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AttackType = AttackType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AttackType = AttackType.Mine;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            AttackMode = AttackMode.Self;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            AttackMode = AttackMode.Drone;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            AttackMode = AttackMode.Turret;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            QueueAttack();
        }

        CollectTargetingInput();
    }

    public void QueueAttack()
    {
        if ((AttackType & AttackType.Bullet) > 0)
        {
            FireBullet = true;
        }
        if ((AttackType & AttackType.Rocket) > 0)
        {
            FireRocket = true;
        }
        if ((AttackType & AttackType.EMP) > 0)
        {
            FireEMP = true;
        }
        if ((AttackType & AttackType.Harpoon) > 0)
        {
            FireHarpoon = true;
        }
        if ((AttackType & AttackType.Flamethrower) > 0)
        {
            FireFlamethrower = true;
        }
        if ((AttackType & AttackType.Laser) > 0)
        {
            FireLaser = true;
        }
        if ((AttackType & AttackType.Mine) > 0)
        {
            FireMine = true;
        }
    }

    private void PlayerReload()
    {
        if ((AttackType & AttackType.Bullet) > 0)
        {
            ReloadBullets();
        }
        if ((AttackType & AttackType.Rocket) > 0)
        {
            ReloadRockets();
        }
        if ((AttackType & AttackType.Mine) > 0)
        {
            ReloadMines();
        }
        if (AttackMode == AttackMode.Drone)
        {
            ReloadDrones();
        }
        if (AttackMode == AttackMode.Turret)
        {
            ReloadTurrets();
        }
    }

    public void ReloadBullets()
    {
        if (Scrap >= 10)
        {
            Bullets += 10;
            Scrap -= 10;
        }
    }
    
    public void ReloadRockets()
    {
        if (Scrap >= 10)
        {
            Rockets += 10;
            Scrap -= 10;
        }
    }

    public void ReloadMines()
    {
        if (Scrap >= 10)
        {
            Mines += 10;
            Scrap -= 10;
        }
    }

    public void ReloadDrones()
    {
        if (Scrap >= 1)
        {
            Drones += 1;
            Scrap -= 1;
        }
    }
    
    public void ReloadTurrets()
    {
        if (Scrap >= 1)
        {
            Turrets += 1;
            Scrap -= 1;
        }
    }

    private void PlayerUpdateVehicleInputs()
    {
        if (VehicleController.VehicleType == VehicleType.Directed)
        {
            PlayerUpdateVehicleInputsDirected();
        }
        else if (VehicleController.VehicleType == VehicleType.Omnidirectional)
        {
            PlayerUpdateVehicleInputsOmnidirectional();
        }
    }

    private void PlayerUpdateVehicleInputsDirected()
    {
        float turnInput = -Input.GetAxis("Horizontal");
        bool thrustInput = Input.GetKey(KeyCode.W);
        bool breakInput = Input.GetKey(KeyCode.S);

        if (turnInput != 0f
            || thrustInput
            || breakInput)
        {
            Autopilot.Standby();
        }

        if (Autopilot.OnStandby)
        {
            DirectedVehicleController controller = (DirectedVehicleController)VehicleController;
            controller.TurnInput = turnInput;
            controller.ThrustInput = thrustInput;
            controller.BreakInput = breakInput;
        }
    }

    private void PlayerUpdateVehicleInputsOmnidirectional()
    {
        Vector2 thrustInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            thrustInput += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            thrustInput += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            thrustInput += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            thrustInput += Vector2.right;
        }

        bool breakInput = Input.GetKey(KeyCode.F);

        if (thrustInput != Vector2.zero
            || breakInput)
        {
            Autopilot.Standby();
        }

        if (Autopilot.OnStandby)
        {
            OmnidirectionalVehicleController controller = (OmnidirectionalVehicleController)VehicleController;
            controller.ThrustInput = thrustInput;
            controller.BreakInput = breakInput;
        }
    }

    private void CollectTargetingInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleTarget();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DropTarget();
        }
    }

    private void ValidateTarget()
    {
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
    
    private void UpdateLifeSupport()
    {
        float energyCost = LifeSupportEnergy * Time.deltaTime;
        if (CurrentEnergy >= energyCost)
        {
            CurrentEnergy -= energyCost;
            CurrentHealth += HealthRegen * Time.fixedDeltaTime;
        }
        else
        {
            TakeDamage(null, LifeSupportDegen, DamageType.Physical);
            CurrentEnergy = 0;
        }
    }

    private void UpdateVehicle()
    {
        float energyCost = ThrustEnergy * Time.fixedDeltaTime;
        Thrusting = VehicleController.UpdateVehicle(CurrentEnergy >= energyCost);
        if (Thrusting)
        {
            CurrentEnergy -= energyCost;
        }
    }

    private void ExecuteAttacks()
    {
        if (FireBullet && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            if ((AttackMode & AttackMode.Self) > 0 && Bullets >= 1)
            {
                Bullets--;
                int damage = Mathf.RoundToInt(Random.Range(60, 100));
                new BulletAttackManager(this, VehicleController.Position, AttackVector(), VehicleController.Velocity, damage);
                VehicleController.ApplyRecoil(-AttackVector() * BulletAttackManager.Recoil);
            }
            else if ((AttackMode & AttackMode.Drone) > 0 && Bullets >= 10)
            {
                Bullets -= 10;
                SpawnDrone(AttackType.Bullet);
            }
            else if ((AttackMode & AttackMode.Turret) > 0 && Bullets >= 10)
            {
                Bullets -= 10;
                SpawnTurret(AttackType.Bullet);
            }
        }
        if (FireRocket && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            CurrentEnergy -= AttackEnergy;
            if ((AttackMode & AttackMode.Self) > 0 && Rockets >= 1)
            {
                Rockets--;
                int damage = Mathf.RoundToInt(Random.Range(10f, 30f));
                new RocketAttackManager(this, HasValidTarget, TargetUID, VehicleController.Position, AttackVector(), VehicleController.Velocity, damage);
                VehicleController.ApplyRecoil(-AttackVector() * RocketAttackManager.Recoil);
            }
            else if ((AttackMode & AttackMode.Drone) > 0 && Rockets >= 10)
            {
                Rockets -= 10;
                SpawnDrone(AttackType.Rocket);
            }
            else if ((AttackMode & AttackMode.Turret) > 0 && Rockets >= 10)
            {
                Rockets -= 10;
                SpawnTurret(AttackType.Rocket);
            }
        }
        if (FireEMP && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            if ((AttackMode & AttackMode.Self) > 0)
            {
                CurrentEnergy -= AttackEnergy;
                int damage = Mathf.RoundToInt(Random.Range(30f, 60f));
                new EMPAttackManager(this, VehicleController.Position, damage);
            }
            else if ((AttackMode & AttackMode.Drone) > 0)
            {
                SpawnDrone(AttackType.EMP);
            }
            else if ((AttackMode & AttackMode.Turret) > 0)
            {
                SpawnTurret(AttackType.EMP);
            }
        }
        if (FireHarpoon && CurrentEnergy >= AttackEnergy && !HarpoonDeployed() && AttackCooldown.Use())
        {
            if ((AttackMode & AttackMode.Self) > 0)
            {
                CurrentEnergy -= AttackEnergy;
                int damage = Mathf.RoundToInt(Random.Range(20f, 50f));
                Harpoon = new HarpoonAttackManager(this, VehicleController.Position, AttackVector(), VehicleController.Velocity, damage);
            }
            else if ((AttackMode & AttackMode.Drone) > 0)
            {
                SpawnDrone(AttackType.Harpoon);
            }
            else if ((AttackMode & AttackMode.Turret) > 0)
            {
                SpawnTurret(AttackType.Harpoon);
            }
        }
        if (FireFlamethrower && CurrentEnergy >= AttackEnergy * Time.fixedDeltaTime)
        {
            if ((AttackMode & AttackMode.Self) > 0)
            {
                CurrentEnergy -= AttackEnergy * Time.fixedDeltaTime;
                Flamethrower.TurnOn(AttackVector());
            }
            else if ((AttackMode & AttackMode.Drone) > 0 && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
            {
                SpawnDrone(AttackType.Flamethrower);
            }
            else if ((AttackMode & AttackMode.Turret) > 0 && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
            {
                SpawnTurret(AttackType.Flamethrower);
            }
        }
        else
        {
            Flamethrower.TurnOff();
        }
        if (FireLaser && CurrentEnergy >= AttackEnergy * Time.fixedDeltaTime)
        {
            if ((AttackMode & AttackMode.Self) > 0)
            {
                CurrentEnergy -= AttackEnergy * Time.fixedDeltaTime;
                Laser.TurnOn(AttackVector(), HasValidTarget, TargetUID);
            }
            else if ((AttackMode & AttackMode.Drone) > 0 && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
            {
                SpawnDrone(AttackType.Laser);
            }
            else if ((AttackMode & AttackMode.Turret) > 0 && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
            {
                SpawnTurret(AttackType.Laser);
            }
        }
        else
        {
            Laser.TurnOff();
        }
        if (FireMine && CurrentEnergy >= AttackEnergy && AttackCooldown.Use())
        {
            if ((AttackMode & AttackMode.Self) > 0 && Mines >= 1)
            {
                Mines--;
                CurrentEnergy -= AttackEnergy;
                int damage = Mathf.RoundToInt(Random.Range(20f, 50f));
                new MineAttackManager(this, VehicleController.Position, damage);
            }
            else if ((AttackMode & AttackMode.Drone) > 0 && Mines >= 10)
            {
                Mines -= 10;
                SpawnDrone(AttackType.Mine);
            }
            else if ((AttackMode & AttackMode.Turret) > 0 && Mines >= 10)
            {
                Mines -= 10;
                SpawnTurret(AttackType.Mine);
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateImmunities();
        UpdateEnergyRegen();
        UpdateLifeSupport();
        UpdateShieldDrain();
        UpdateVehicle();
        SubmitRadarProfile();
        ExecuteAttacks();  
    }

    private void UpdateEnergyRegen()
    {
        if (CurrentEnergy < MaxEnergy)
        {
            float fuelCost = FuelUsage * Time.fixedDeltaTime;
            if (CurrentFuel >= fuelCost)
            {
                CurrentFuel -= fuelCost;
                CurrentFuel = Mathf.Clamp(CurrentFuel, 0, MaxFuel);
                CurrentEnergy += EnergyRegen * Time.fixedDeltaTime;
                CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
            }
            else
            {
                CurrentFuel = 0f;
            }
        }
    }

    private void UpdateShieldDrain()
    {
        if (CurrentShield < MaxShield)
        {
            float energyCost = ShieldEnergy * Time.fixedDeltaTime;
            if (CurrentEnergy >= energyCost)
            {
                CurrentEnergy -= energyCost;
                CurrentShield += ShieldRegen * Time.fixedDeltaTime;
                CurrentShield = Mathf.Clamp(CurrentShield, 0, MaxShield);
            }
        }
    }

    private void SpawnDrone(AttackType attackType)
    {
        if (Drones >= 1)
        {
            Drones--;
            SpaceshipParameters parameters = SpaceshipTable.PrepareSpaceshipParameters(SpaceshipModel.Alpha1, "(" + Name + ")'s drone #" + (++NumberOfDrones), Team);
            parameters.AIType = AIType.DroneAI;
            parameters.AIParameters[0] = attackType.ToString();
            GameObject spaceship = GameManager.Instance.Instantiate(SpaceshipPrefabs.Instance.Prefabs[SpaceshipModel.Alpha1], Position, Quaternion.Euler(0, 0, AttackAngle()));
            spaceship.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            spaceship.GetComponent<Spaceship>().Initialize(null, this, parameters);
            spaceship.GetComponent<Spaceship>().Scrap = 10;
        }
    }

    private void SpawnTurret(AttackType attackType)
    {
        if (Turrets >= 1)
        {
            Turrets--;
            SpaceshipParameters parameters = SpaceshipTable.PrepareSpaceshipParameters(SpaceshipModel.Turret, "(" + Name + ")'s turret #" + (++NumberOfTurrets), Team);
            parameters.AIParameters[0] = attackType.ToString();
            GameObject spaceship = GameManager.Instance.Instantiate(SpaceshipPrefabs.Instance.Prefabs[SpaceshipModel.Turret], Position, Quaternion.Euler(0, 0, AttackAngle()));
            spaceship.GetComponent<Spaceship>().Initialize(null, this, parameters);
            spaceship.GetComponent<Spaceship>().Scrap = 10;
        }
    }

    protected void ZeroAttackInputs()
    {
        FireBullet = false;
        FireRocket = false;
        FireEMP = false;
        FireHarpoon = false;
        FireFlamethrower = false;
        FireLaser = false;
        FireMine = false;
    }

    private void SubmitRadarProfile()
    {
        RadarProfile profile = new RadarProfile(
            UID,
            Name,
            Team,
            PlayerControlled,
            Position,
            MaxShield,
            CurrentShield,
            MaxHealth,
            CurrentHealth,
            MaxEnergy,
            CurrentEnergy,
            MaxFuel,
            CurrentFuel);
        RadarOmniscience.Instance.SubmitRadarProfile(UID, profile);
    }

    protected virtual void Die()
    {
        CurrentHealth = 0f;
        CurrentShield = 0f;
        CurrentEnergy = 0f;
        CurrentFuel = 0f;

        RadarOmniscience.Instance.UnregisterRadarEntity(UID);
        SpaceshipRegistry.Instance.UnregisterSpaceship(UID);
        Destroy(gameObject);
        DropLoot();
        if (PlayerControlled)
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    private void DropLoot()
    {
        if (Team == 1)
        {
            for (int i = 0; i < LootExperience; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.ExpMorsel, VehicleController.Position, Quaternion.identity);
            }
            for (int i = 0; i < LootCredits; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Coin, VehicleController.Position, Quaternion.identity);
            }
            for (int i = 0; i < LootFuel; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FuelRod, VehicleController.Position, Quaternion.identity);
            }
            for (int i = 0; i < LootScrap; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Scrap, VehicleController.Position, Quaternion.identity);
            }
            for (int i = 0; i < LootItems; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Crate, VehicleController.Position, Quaternion.identity);
            }
        }
    }

    public virtual void PickupExp(int quantity)
    {
        if (PlayerControlled)
        {
            Player.EarnExperience(quantity);
        }
    }

    public virtual void PickupCredits(float quantity)
    {
        Credits += quantity;
    }

    public virtual void PickupFuel(float quantity)
    {
        CurrentFuel += quantity;
        CurrentFuel = Mathf.Clamp(CurrentFuel, 0, MaxFuel);
    }

    public virtual void PickupScrap(float quantity)
    {
        Scrap += quantity;
    }

    public virtual void PickupCrate(int quantity)
    {

    }

    public void SelectTarget(int uid)
    {
        HasTarget = true;
        TargetUID = uid;
    }

    public void DropTarget()
    {
        HasTarget = false;
    }

    public Dictionary<int, RadarProfile> GetRadarReading()
    {
        return RadarOmniscience.Instance.PingRadar(UID);
    }

    private bool HarpoonDeployed()
    {
        if (Harpoon == null)
        {
            return false;
        }
        else
        {
            return (Harpoon.State != HarpoonState.Expired);
        }
    }

    private Vector2 AttackVector()
    {
        if (TargetingType == TargetingType.Bound)
        {
            return ((DirectedVehicleController)VehicleController).HeadingVector;
        }
        else if (TargetingType == TargetingType.Unbound)
        {
            if (PlayerControlled)
            {
                return (MasterCameraController.GetMousePosition() - Position).normalized;
            }
            else if (HasValidTarget)
            {
                return (GetRadarReading()[TargetUID].Position - Position).normalized;
            }
            else
            {
                return Vector2.zero;
            }
        }
        else
        {
            return Vector2.zero;
        }
    }

    private float AttackAngle()
    {
        return Vector2.SignedAngle(Vector2.right, AttackVector());
    }
}
