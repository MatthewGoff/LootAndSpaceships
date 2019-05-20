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
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha3, new Vector2(0f, 0f), Quaternion.identity);
            Alpha3Controller controller = spaceship.GetComponent<Alpha3Controller>();
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

        GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(10f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Alpha1Controller controller = spaceship.GetComponent<Alpha1Controller>();
        controller.Initialize(null, "Enemy 1", AIType.PassiveAI, AttackType.Bullet, null, true, 1, TargetingType.Bound);
        Enemies.Add(controller);
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
