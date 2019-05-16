using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AutopilotTargetEffect;

    public bool PlayerAlive
    {
        get
        {
            return PlayerController != null;
        }
    }

    public PlayerController PlayerController;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnNewEnemy();
        }
        if (Input.GetKeyDown(KeyCode.R) && !PlayerAlive)
        {
            SpawnPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerController.PickupExp(1);
        }
    }

    public void PlayerLevelUp(int level)
    {
        Instantiate(Prefabs.Instance.LevelUpBlast, Vector2.zero, Quaternion.identity);
        LevelUpText.GetComponent<LevelUpTextController>().Display(level);
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(Prefabs.Instance.Player, new Vector2(0f, 0f), Quaternion.identity);
        PlayerController = player.GetComponent<PlayerController>();
        PlayerController.Initialize("Player 1");
        PlayerController.AutopilotTargetEffect = AutopilotTargetEffect;
    }

    private void SpawnNewEnemy()
    {
        EnemyCounter++;

        GameObject enemy = Instantiate(Prefabs.Instance.Enemy, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
        enemy.GetComponent<EnemyController>().Initialize("Enemy "+EnemyCounter.ToString());
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
