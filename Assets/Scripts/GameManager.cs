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
        if (Input.GetKeyDown(KeyCode.Q) && !PlayerAlive)
        {
            SpawnPlayer(VehicleType.Directed);
        }
        if (Input.GetKeyDown(KeyCode.E) && !PlayerAlive)
        {
            SpawnPlayer(VehicleType.Omnidirectional);
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

    private void SpawnPlayer(VehicleType vehicleType)
    {
        if (vehicleType == VehicleType.Directed)
        {
            GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
            controller.Initialize(ControlType.Player, "Player 1", false, 0);
            PlayerController = controller;
        }
        else if (vehicleType == VehicleType.Omnidirectional)
        {
            GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1Omni, new Vector2(0f, 0f), Quaternion.identity);
            Alpha1OmniController controller = spaceship.GetComponent<Alpha1OmniController>();
            controller.Initialize(ControlType.Player, "Player 1", false, 0);
            PlayerController = controller;
        }
    }

    private void SpawnNewEnemy()
    {
        EnemyCounter++;

        GameObject spaceship = Instantiate(Prefabs.Instance.Alpha1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
        controller.Initialize(ControlType.NPC, "Enemy " + EnemyCounter.ToString(), true, 1);
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
