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

    public Spaceship PlayerController;
    public GameObject LevelUpText;

    private int EnemyCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RadarOmniscience.Initialize();
        SpaceshipRegistry.Initialize();

        EnemyCounter = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !PlayerAlive)
        {
            SpawnPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !PlayerAlive)
        {
            SpawnPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !PlayerAlive)
        {
            SpawnPlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnNewEnemy();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayerLevelUp(int level)
    {
        Instantiate(Prefabs.Instance.LevelUpBlast, Vector2.zero, Quaternion.identity);
        LevelUpText.GetComponent<LevelUpTextController>().Display(level);
    }

    private void SpawnPlayer(int selection)
    {
        if (selection == 1)
        {
            GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize("Player 1", AIType.Player, 0, null, false, 0, TargetingType.Bound);
            PlayerController = controller;
        }
        else if (selection == 2)
        {
            GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize("Player 1", AIType.Player, 0, null, false, 0, TargetingType.Unbound);
            PlayerController = controller;
        }
        else if (selection == 3)
        {
            GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1Omni, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1OmniController controller = spaceship.GetComponent<Alpha1OmniController>();
            controller.Initialize("Player 1", AIType.Player, 0, null, false, 0, TargetingType.Unbound);
            PlayerController = controller;
        }
    }

    private void SpawnNewEnemy()
    {
        EnemyCounter++;

        GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
        controller.Initialize("Enemy " + EnemyCounter.ToString(), AIType.PassiveAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
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
}
