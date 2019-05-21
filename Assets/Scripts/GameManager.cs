using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool PlayerAlive
    {
        get
        {
            return PlayerController != null;
        }
    }

    public GameObject LevelButtons;
    public Player Player;
    public Spaceship PlayerController;
    public GameObject LevelUpText;
    public Transform Scene;
    private Transform VolatileSceneObjects;
    private List<Spaceship> Enemies;
    private int PlayerParadigm;

    private int CurrentLevel;
    private bool SceneActive;

    private void Awake()
    {
        Instance = this;
        Player = new Player();
        PlayerParadigm = 1;
    }

    private void Start()
    {
        LevelButtons.GetComponent<LevelButtonsController>().UnlockButton(1);
        SelectLevel(1);
    }

    private void OpenScene(int level)
    {
        RadarOmniscience.Initialize();
        SpaceshipRegistry.Initialize();
        VolatileSceneObjects = new GameObject("VolatileSceneObjects").transform;
        VolatileSceneObjects.transform.SetParent(Scene);

        SpawnPlayer();
        SpawnEnemies(level);
        SceneActive = true;
    }

    private void CloseScene()
    {
        Destroy(VolatileSceneObjects.gameObject);
        SceneActive = false;
    }

    private void Update()
    {
        if (SceneActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !PlayerAlive)
            {
                PlayerParadigm = 1;
                SpawnPlayer();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && !PlayerAlive)
            {
                PlayerParadigm = 2;
                SpawnPlayer();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && !PlayerAlive)
            {
                PlayerParadigm = 3;
                SpawnPlayer();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        CheckLevelCleared();
    }

    public void PlayerLevelUp(int level)
    {
        Instantiate(GeneralPrefabs.Instance.LevelUpBlast, Vector2.zero, Quaternion.identity);
        LevelUpText.GetComponent<LevelUpTextController>().Display(level);
    }

    private void SpawnPlayer()
    {
        if (PlayerParadigm == 1)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize(Player, "Player 1", AIType.Player, 0, null, false, 0, TargetingType.Bound);
            PlayerController = controller;
        }
        else if (PlayerParadigm == 2)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize(Player, "Player 1", AIType.Player, 0, null, false, 0, TargetingType.Unbound);
            PlayerController = controller;
        }
        else if (PlayerParadigm == 3)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1Omni, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1OmniController controller = spaceship.GetComponent<Alpha1OmniController>();
            controller.Initialize(Player, "Player 1", AIType.Player, 0, null, false, 0, TargetingType.Unbound);
            PlayerController = controller;
        }
    }

    private void SpawnEnemies(int level)
    {
        Enemies = new List<Spaceship>();

        if (level == 1)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize(null, "Enemy 1 [Alpha 1]", AIType.SimpleAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
        }
        else if (level == 2)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha2, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Alpha2Controller controller = spaceship.GetComponent<Alpha2Controller>();
            controller.Initialize(null, "Enemy 1 [Alpha 2]", AIType.SimpleAI, AttackType.Rocket, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha3, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Alpha3Controller controller2 = spaceship.GetComponent<Alpha3Controller>();
            controller2.Initialize(null, "Enemy 2 [Alpha 3]", AIType.SimpleAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha3, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Alpha3Controller controller3 = spaceship.GetComponent<Alpha3Controller>();
            controller3.Initialize(null, "Enemy 3 [Alpha 3]", AIType.SimpleAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 3)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Beta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Beta1Controller controller = spaceship.GetComponent<Beta1Controller>();
            controller.Initialize(null, "Enemy 1 [Beta 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha4, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Alpha4Controller controller2 = spaceship.GetComponent<Alpha4Controller>();
            controller2.Initialize(null, "Enemy 2 [Alpha 4]", AIType.SimpleAI, AttackType.EMP, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha4, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Alpha4Controller controller3 = spaceship.GetComponent<Alpha4Controller>();
            controller3.Initialize(null, "Enemy 3 [Alpha 4]", AIType.SimpleAI, AttackType.EMP, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 4)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Eta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Eta1Controller controller = spaceship.GetComponent<Eta1Controller>();
            controller.Initialize(null, "Enemy 1 [Eta 1]", AIType.SimpleAI, AttackType.Laser, null, true, 1, TargetingType.Bound);
            controller.AttackMode = AttackMode.Turret;
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Delta1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Delta1Controller controller2 = spaceship.GetComponent<Delta1Controller>();
            controller2.Initialize(null, "Enemy 2 [Delta 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Delta1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Delta1Controller controller3 = spaceship.GetComponent<Delta1Controller>();
            controller3.Initialize(null, "Enemy 3 [Delta 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 5)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Gamma1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Gamma1Controller controller = spaceship.GetComponent<Gamma1Controller>();
            controller.Initialize(null, "Enemy 1 [Gamma 1]", AIType.SimpleAI, AttackType.Flamethrower, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Iota1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Iota1Controller controller2 = spaceship.GetComponent<Iota1Controller>();
            controller2.Initialize(null, "Enemy 2 [Iota 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Iota1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Iota1Controller controller3 = spaceship.GetComponent<Iota1Controller>();
            controller3.Initialize(null, "Enemy 3 [Iota 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 6)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Omikron1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Omikron1Controller controller = spaceship.GetComponent<Omikron1Controller>();
            controller.Initialize(null, "Enemy 1 [Omikron 1]", AIType.SimpleAI, AttackType.Laser, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Nu1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Nu1Controller controller2 = spaceship.GetComponent<Nu1Controller>();
            controller2.Initialize(null, "Enemy 2 [Nu 1]", AIType.SimpleAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Nu1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Nu1Controller controller3 = spaceship.GetComponent<Nu1Controller>();
            controller3.Initialize(null, "Enemy 3 [Nu 1]", AIType.SimpleAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 7)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Phi1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Phi1Controller controller = spaceship.GetComponent<Phi1Controller>();
            controller.Initialize(null, "Enemy 1 [Phi 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            controller.AttackMode = AttackMode.Drone;
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Pi1Controller controller2 = spaceship.GetComponent<Pi1Controller>();
            controller2.Initialize(null, "Enemy 2 [Pi 1]", AIType.SimpleAI, AttackType.Rocket, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Pi1Controller controller3 = spaceship.GetComponent<Pi1Controller>();
            controller3.Initialize(null, "Enemy 3 [Pi 1]", AIType.SimpleAI, AttackType.Rocket, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 8)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi2, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Pi2Controller controller = spaceship.GetComponent<Pi2Controller>();
            controller.Initialize(null, "Enemy 1 [Pi 2]", AIType.SimpleAI, AttackType.Flamethrower, null, true, 1, TargetingType.Bound);
            controller.AttackMode = AttackMode.Drone;
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Sigma1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Sigma1Controller controller2 = spaceship.GetComponent<Sigma1Controller>();
            controller2.Initialize(null, "Enemy 2 [Sigma 1]", AIType.SimpleAI, AttackType.Rocket, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Sigma1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Sigma1Controller controller3 = spaceship.GetComponent<Sigma1Controller>();
            controller3.Initialize(null, "Enemy 3 [Sigma 1]", AIType.SimpleAI, AttackType.Rocket, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 9)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Theta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Theta1Controller controller = spaceship.GetComponent<Theta1Controller>();
            controller.Initialize(null, "Enemy 1 [Theta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            controller.AttackMode = AttackMode.Turret;
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Tau1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Tau1Controller controller2 = spaceship.GetComponent<Tau1Controller>();
            controller2.Initialize(null, "Enemy 2 [Tau 1]", AIType.SimpleAI, AttackType.Flamethrower, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Tau1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Tau1Controller controller3 = spaceship.GetComponent<Tau1Controller>();
            controller3.Initialize(null, "Enemy 3 [Tau 1]", AIType.SimpleAI, AttackType.Flamethrower, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller3);
        }
        else if (level == 10)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Upsilon1, new Vector2(50f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Upsilon1Controller controller = spaceship.GetComponent<Upsilon1Controller>();
            controller.Initialize(null, "Enemy 1 [Upsilon 1]", AIType.SimpleAI, AttackType.Laser, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(40f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Xi1Controller controller2 = spaceship.GetComponent<Xi1Controller>();
            controller2.Initialize(null, "Enemy 2 [Xi 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            controller2.AttackMode = AttackMode.Drone;
            Enemies.Add(controller2);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(50f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Xi1Controller controller3 = spaceship.GetComponent<Xi1Controller>();
            controller3.Initialize(null, "Enemy 3 [Xi 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            controller3.AttackMode = AttackMode.Drone;
            Enemies.Add(controller3);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(50f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Xi1Controller controller4 = spaceship.GetComponent<Xi1Controller>();
            controller4.Initialize(null, "Enemy 4 [Xi 1]", AIType.SimpleAI, AttackType.Mine, null, true, 1, TargetingType.Bound);
            controller4.AttackMode = AttackMode.Drone;
            Enemies.Add(controller4);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Zeta1Controller controller5 = spaceship.GetComponent<Zeta1Controller>();
            controller5.Initialize(null, "Enemy 5 [Zeta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller5);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(40f, -5f), Quaternion.Euler(0f, 0f, 180f));
            Zeta1Controller controller6 = spaceship.GetComponent<Zeta1Controller>();
            controller6.Initialize(null, "Enemy 6 [Zeta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller6);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(40f, 5f), Quaternion.Euler(0f, 0f, 180f));
            Zeta1Controller controller7 = spaceship.GetComponent<Zeta1Controller>();
            controller7.Initialize(null, "Enemy 7 [Zeta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller7);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(50f, -10f), Quaternion.Euler(0f, 0f, 180f));
            Zeta1Controller controller8 = spaceship.GetComponent<Zeta1Controller>();
            controller8.Initialize(null, "Enemy 8 [Zeta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller8);
            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(50f, 10f), Quaternion.Euler(0f, 0f, 180f));
            Zeta1Controller controller9 = spaceship.GetComponent<Zeta1Controller>();
            controller9.Initialize(null, "Enemy 9 [Zeta 1]", AIType.SimpleAI, AttackType.Harpoon, null, true, 1, TargetingType.Bound);
            Enemies.Add(controller9);
        }
    }

    public GameObject Instantiate(GameObject prefab, Vector2 position, Quaternion rotation, Transform transform = null)
    {
        GameObject gameObject = GameObject.Instantiate(prefab, position, rotation);
        if (transform != null)
        {
            gameObject.transform.SetParent(transform);
        }
        else
        {
            gameObject.transform.SetParent(VolatileSceneObjects);
        }
        return gameObject;
    }

    public Transform CreateTransform(string name)
    {
        GameObject gameObject = new GameObject(name);
        gameObject.transform.SetParent(VolatileSceneObjects);
        return gameObject.transform;
    }

    public static bool MouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    public void SelectPlayerTarget(int uid)
    {
        if (PlayerAlive)
        {
            PlayerController.SelectTarget(uid);
        }
    }

    public void PlayerDeath()
    {
        PlayerController = null;
    }

    public void CheckLevelCleared()
    {
        if (SpaceshipRegistry.Instance.CountTeamMembers(1) == 0)
        {
            LevelCleared();
        }
    }

    private void LevelCleared()
    {
        LevelButtons.GetComponent<LevelButtonsController>().Show();
        if (CurrentLevel < 10)
        {
            LevelButtons.GetComponent<LevelButtonsController>().UnlockButton(CurrentLevel + 1);
        }
    }

    public void SelectLevel(int level)
    {
        if (SceneActive)
        {
            CloseScene();
        }
        CurrentLevel = level;
        LevelButtons.GetComponent<LevelButtonsController>().Hide();
        OpenScene(CurrentLevel);
    }
}
