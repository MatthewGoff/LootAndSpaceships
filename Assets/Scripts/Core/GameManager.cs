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
    public GameObject RootCanvas;
    private Transform VolatileSceneObjects;

    private int CurrentLevel;
    private bool SceneActive;

    private void Awake()
    {
        Instance = this;
        SpaceshipTable.Initialize();
    }

    private void Start()
    {
        Player = new Player();
        LevelButtons.GetComponent<LevelButtonsController>().UnlockButton(1);
        SelectLevel(1);
    }

    private void OpenScene(int level)
    {
        Omniscience.Initialize();
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
                SpawnPlayer();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 1; i <= 10; i++)
            {
                LevelButtons.GetComponent<LevelButtonsController>().UnlockButton(i);
            }
            LevelButtons.GetComponent<LevelButtonsController>().Show();
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
        GameObject prefab = SpaceshipPrefabs.Instance.Prefabs[SpaceshipTable.Instance.GetHullParameters("Player").Model];
        GameObject gameObject = Instantiate(prefab, Vector2.zero, Quaternion.identity);
        Spaceship spaceship = gameObject.GetComponent<Spaceship>();
        int UID = Omniscience.Instance.RegisterNewEntity(spaceship);
        Liscense liscense = new Liscense(UID, "Player 1", 0, Player, null);
        spaceship.Initialize(liscense, "Player");
        PlayerController = spaceship;
    }

    private void SpawnEnemies(int level)
    {
        Dictionary<string, int> spawnNumbers = SpaceshipTable.Instance.GetSpawnQuantities(level);

        int enemyCount = 0;
        foreach (string configuration in spawnNumbers.Keys)
        {
            for (int i = 0; i < spawnNumbers[configuration]; i++)
            {
                SpawnEnemy(configuration, ++enemyCount);
            }
        }
    }

    private void SpawnEnemy(string configuration, int enemyCount)
    {
        Vector2 spawnLocation = new Vector2(40, 0) + 10 * Random.insideUnitCircle;
        GameObject prefab = SpaceshipPrefabs.Instance.Prefabs[SpaceshipTable.Instance.GetHullParameters(configuration).Model];
        GameObject gameObject = Instantiate(prefab, spawnLocation, Quaternion.Euler(0f, 0f, 180f));
        Spaceship spaceship = gameObject.GetComponent<Spaceship>();
        int UID = Omniscience.Instance.RegisterNewEntity(spaceship);
        Liscense liscense = new Liscense(UID, "Enemy " + enemyCount.ToString(), 1, null, null);
        spaceship.Initialize(liscense, configuration);
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
        if (Omniscience.Instance.CountTeamMembers(1) == 0)
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
