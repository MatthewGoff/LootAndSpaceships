using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : Vehicle
{
    public GameObject PortraitCamera;
    public GameObject FireEffect;

    public float Credits;
    public float Scrap;
    public int Bullets;
    public int Rockets;
    public int Mines;
    public int Drones;
    public int Turrets;

    public bool HasTarget;
    public int TargetUID;
    public bool HasValidTarget
    {
        get
        {
            ValidateTarget();
            return HasTarget;
        }
    }

    private bool ShowFDN;

    private float BurnEndTime;
    private bool Burning;

    private List<AttackImmunityRecord> Immunities;
    public Autopilot Autopilot;
    private AI AI;
    protected bool Thrusting;
    private bool[] WeaponInputs;
    private int SelectedWeapon;

    private Dictionary<FlotsamType, int> LootDrops;

    private Liscense Liscense;
    private Hull Hull;
    private Engine Engine;
    private Reactor Reactor;
    private ShieldGenerator ShieldGenerator;
    private LifeSupport LifeSupport;
    private Weapon[] Weapons;

    public bool PlayerControlled
    {
        get
        {
            return AI == null;
        }
    }
    public int Team
    {
        get
        {
            return Liscense.Team;
        }
    }
    public string Name
    {
        get
        {
            return Liscense.Name;
        }
    }
    public float HullSpace
    {
        get
        {
            return Hull.HullSpace;
        }
    }

    public void Initialize(Liscense liscense, string configuration)
    {
        WeaponParameters[] weaponParameters = SpaceshipTable.Instance.GetWeaponParameters(configuration);
        Weapon[] weapons = new Weapon[weaponParameters.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = Weapon.CreateWeapon(weaponParameters[i]);
        }
        Initialize(
            liscense,
            SpaceshipTable.Instance.GetAIParameters(configuration),
            SpaceshipTable.Instance.GetLootDrops(configuration),
            new Hull(SpaceshipTable.Instance.GetHullParameters(configuration)),
            new Engine(SpaceshipTable.Instance.GetEngineParameters(configuration)),
            new Reactor(SpaceshipTable.Instance.GetReactorParameters(configuration)),
            new ShieldGenerator(SpaceshipTable.Instance.GetShieldGeneratorParameters(configuration)),
            new LifeSupport(SpaceshipTable.Instance.GetLifeSupportParameters(configuration)),
            weapons
        );
    }

    public void Initialize
    (
        Liscense liscense,
        AIParameters aiParameters,
        Dictionary<FlotsamType, int> lootDrops,
        Hull hull,
        Engine engine,
        Reactor reactor,
        ShieldGenerator shieldGenerator,
        LifeSupport lifeSupport,
        Weapon[] weapons
    )
    {
        Hull = hull;
        Engine = engine;
        Reactor = reactor;
        ShieldGenerator = shieldGenerator;
        LifeSupport = lifeSupport;
        Weapons = weapons;
        WeaponInputs = new bool[Weapons.Length];

        base.Initialize
        (
            thrustForce: Engine.ThrustForceMultiplier * Mathf.PI * Mathf.Pow(Hull.Size, 2),
            turnForce: Engine.TurnForceMultiplier * Mathf.PI * Mathf.Pow(Hull.Size, 2),
            maximumSpeed: Engine.MaximumSpeed,
            mass: Hull.Mass
        );

        transform.localScale = new Vector2(Hull.Size, Hull.Size);
        Liscense = liscense;
        SelectedWeapon = 0;
        ShowFDN = (Team != 0);
        Immunities = new List<AttackImmunityRecord>();
        Autopilot = new FastAutopilot(this);
        AI = AI.CreateAI(aiParameters, this, Autopilot, Liscense.Mother);
        LootDrops = lootDrops;
        Credits = 0f;
        Scrap = Hull.HullSpace;
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
            GameObject fdn = GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FDN, Position, Quaternion.identity);
            fdn.GetComponent<FDNController>().Display(Mathf.RoundToInt(damage), damage / 100f);
        }

        if (damageType == DamageType.Explosion || damageType == DamageType.Fire)
        {
            BurnEndTime = Time.time + LifeSupport.BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }

        float shieldDamage = Mathf.Min(ShieldGenerator.CurrentHitPoints, damage);
        ShieldGenerator.CurrentHitPoints -= shieldDamage;
        float remainingDamage = damage - shieldDamage;
        Hull.CurrentHitPoints -= remainingDamage;
        Hull.CurrentHitPoints = Mathf.Clamp(Hull.CurrentHitPoints, 0, Hull.MaximumHitPoints);
        if (Hull.CurrentHitPoints == 0)
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
            GameManager.Instance.SelectPlayerTarget(Liscense.UID);
        }
    }

    private void Update()
    {
        ZeroWeaponInputs();

        if (PlayerControlled)
        {
            PlayerUpdate();
        }
        else
        {
            AI.Update(Omniscience.Instance.PingRadar(Liscense.UID));
        }

        bool destinationReached = Autopilot.Update();
        if (destinationReached && PlayerControlled)
        {
            Autopilot.Standby();
        }

        ModelSpecificUpdate();
    }

    private void ZeroWeaponInputs()
    {
        for (int i = 0; i < WeaponInputs.Length; i++)
        {
            WeaponInputs[i] = false;
        }
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

        PlayerUpdateVehicleInputs();

        if (Input.GetMouseButton(1) && !GameManager.MouseOverUI())
        {
            Vector2 target = MasterCameraController.GetMousePosition();
            Autopilot.SetTarget(target, AutopilotBehaviour.Seek);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SelectedWeapon = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SelectedWeapon = 2;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            QueueAttack(SelectedWeapon);
        }

        CollectTargetingInput();
    }

    public void QueueAttack(int weaponSlot)
    {
        WeaponInputs[weaponSlot - 1] = true;
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
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            turnInput = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = -1f;
        }
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
            TurnInput = turnInput;
            ThrustInput = thrustInput;
            BreakInput = breakInput;
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
        if (!Omniscience.Instance.PingRadar().ContainsKey(TargetUID))
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
        float energyCost = LifeSupport.EnergyUsage * Time.deltaTime;
        if (Reactor.CurrentEnergy >= energyCost)
        {
            Reactor.CurrentEnergy -= energyCost;
        }
        else
        {
            TakeDamage(null, LifeSupport.Degen, DamageType.Physical);
            Reactor.CurrentEnergy = 0;
        }
    }

    private void UpdateVehicle()
    {
        float energyCost = Engine.ThrustEnergy * Time.fixedDeltaTime;
        Thrusting = UpdateVehicle(Reactor.CurrentEnergy >= energyCost);
        if (Thrusting)
        {
            Reactor.CurrentEnergy -= energyCost;
        }
    }

    private void ExecuteAttacks()
    {
        for (int i = 0; i < WeaponInputs.Length; i++)
        {
            if (WeaponInputs[i]
                && Scrap >= Weapons[i].ScrapCost
                && Reactor.CurrentEnergy >= Weapons[i].MarginalEnergyCost(Time.fixedDeltaTime)
                && Weapons[i].CooldownAvailable())
            {
                Scrap -= Weapons[i].ScrapCost;
                Reactor.CurrentEnergy -= Weapons[i].MarginalEnergyCost(Time.fixedDeltaTime);
                Weapons[i].UseCooldown();
                Weapons[i].ExecuteAttack(this, HasValidTarget, TargetUID, Position, AttackVector(), Velocity);
                ApplyRecoil(-AttackVector() * Weapons[i].Recoil);
            }
            else
            {
                Weapons[i].Cease();
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
        ExecuteAttacks();
        SubmitRadarProfile();
    }

    private void UpdateEnergyRegen()
    {
        float energyDeficit = Reactor.EnergyCapacity - Reactor.CurrentEnergy;
        if (energyDeficit > 0)
        {
            float fuelCost = Mathf.Clamp01(energyDeficit / (Reactor.EnergyRegen * Time.fixedDeltaTime)) * Reactor.FuelConsumption * Time.fixedDeltaTime;
            if (Reactor.CurrentFuel >= fuelCost)
            {
                Reactor.CurrentFuel -= fuelCost;
                Reactor.CurrentFuel = Mathf.Clamp(Reactor.CurrentFuel, 0, Reactor.FuelCapacity);
                Reactor.CurrentEnergy += Reactor.EnergyRegen * Time.fixedDeltaTime;
                Reactor.CurrentEnergy = Mathf.Clamp(Reactor.CurrentEnergy, 0, Reactor.EnergyCapacity);
            }
            else
            {
                Reactor.CurrentFuel = 0f;
            }
        }
    }

    private void UpdateShieldDrain()
    {
        if (ShieldGenerator.CurrentHitPoints < ShieldGenerator.MaximumHitPoints)
        {
            float energyCost = ShieldGenerator.EnergyUsage * Time.fixedDeltaTime;
            if (Reactor.CurrentEnergy >= energyCost)
            {
                Reactor.CurrentEnergy -= energyCost;
                ShieldGenerator.CurrentHitPoints += ShieldGenerator.Regen * Time.fixedDeltaTime;
                ShieldGenerator.CurrentHitPoints = Mathf.Clamp(ShieldGenerator.CurrentHitPoints, 0, ShieldGenerator.MaximumHitPoints);
            }
        }
    }

    private void SubmitRadarProfile()
    {
        Omniscience.Instance.SubmitRadarProfile(Liscense.UID, GetRadarProfile());
    }

    public RadarProfile GetRadarProfile()
    {
        return new RadarProfile
        (
            Liscense.UID,
            Liscense.Name,
            Team,
            PlayerControlled,
            Position,
            ShieldGenerator.MaximumHitPoints,
            ShieldGenerator.CurrentHitPoints,
            Hull.MaximumHitPoints,
            Hull.CurrentHitPoints,
            Reactor.EnergyCapacity,
            Reactor.CurrentEnergy,
            Reactor.FuelCapacity,
            Reactor.CurrentFuel
        );
    }

    protected virtual void Die()
    {
        Hull.CurrentHitPoints = 0f;
        ShieldGenerator.CurrentHitPoints = 0f;
        Reactor.CurrentEnergy = 0f;
        Reactor.CurrentFuel = 0f;

        Omniscience.Instance.UnregisterEntity(Liscense.UID);
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
            for (int i = 0; i < LootDrops[FlotsamType.Experience]; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.ExpMorsel, Position, Quaternion.identity);
            }
            for (int i = 0; i < LootDrops[FlotsamType.Credits]; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Coin, Position, Quaternion.identity);
            }
            for (int i = 0; i < LootDrops[FlotsamType.Fuel]; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.FuelRod, Position, Quaternion.identity);
            }
            for (int i = 0; i < LootDrops[FlotsamType.Scrap]; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Scrap, Position, Quaternion.identity);
            }
            for (int i = 0; i < LootDrops[FlotsamType.Items]; i++)
            {
                GameManager.Instance.Instantiate(GeneralPrefabs.Instance.Crate, Position, Quaternion.identity);
            }
        }
    }

    public virtual void PickupExp(int quantity)
    {
        if (PlayerControlled)
        {
            Liscense.Player.EarnExperience(quantity);
        }
    }

    public virtual void PickupCredits(float quantity)
    {
        Credits += quantity;
    }

    public virtual void PickupFuel(float quantity)
    {
        Reactor.CurrentFuel += quantity;
        Reactor.CurrentFuel = Mathf.Clamp(Reactor.CurrentFuel, 0, Reactor.FuelCapacity);
    }

    public virtual void PickupScrap(float quantity)
    {
        Scrap += quantity;
    }

    public virtual void PickupCrate(int quantity)
    {
        if (PlayerControlled)
        {
            ItemType itemType = Item.GetRandomItemType();
            Item item = Item.CreateRandomItem(0, itemType);
            Liscense.Player.PickupItem(item);
        }
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
        return Omniscience.Instance.PingRadar(Liscense.UID);
    }

    public Vector2 AttackVector()
    {
        if (Hull.TargetingType == TargetingType.Bound)
        {
            return HeadingVector;
        }
        else if (Hull.TargetingType == TargetingType.Unbound)
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

    public float AttackAngle()
    {
        return Vector2.SignedAngle(Vector2.right, AttackVector());
    }
}
