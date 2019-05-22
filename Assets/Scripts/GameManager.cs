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
        SpaceshipParameters parameters = null;
        if (PlayerParadigm == 1)
        {
            parameters = SpaceshipTable.PrepareSpaceshipParameters(SpaceshipModel.Alpha1, "Player 1", 0);
            parameters.AIType = AIType.Player;
        }
        else if (PlayerParadigm == 2)
        {
            parameters = SpaceshipTable.PrepareSpaceshipParameters(SpaceshipModel.Alpha1, "Player 1", 0);
            parameters.AIType = AIType.Player;
            parameters.TargetingType = TargetingType.Unbound;
        }
        else if (PlayerParadigm == 3)
        {
            parameters = SpaceshipTable.PrepareSpaceshipParameters(SpaceshipModel.Alpha1Omni, "Player 1", 0);
            parameters.AIType = AIType.Player;
        }
        GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Prefabs[parameters.Model], new Vector2(0f, 0f), Quaternion.identity);
        PlayerController = spaceship.GetComponent<Spaceship>();
        PlayerController.Initialize(Player, null, parameters);
    }

    private void SpawnEnemies(int level)
    {
        Dictionary<SpaceshipModel, int> spawnNumbers = SpaceshipTable.GetSpawnNumbers(level);

        int enemyCount = 0;
        foreach (SpaceshipModel model in spawnNumbers.Keys)
        {
            for (int i = 0; i < spawnNumbers[model]; i++)
            {
                SpawnEnemy(model, ++enemyCount);
            }
        }
    }

    private void SpawnEnemy(SpaceshipModel model, int enemyCount)
    {
        Vector2 spawnLocation = new Vector2(40, 0) + 10 * Random.insideUnitCircle;
        SpaceshipParameters parameters = SpaceshipTable.PrepareSpaceshipParameters(model, "Enemy " + enemyCount, 1);
        GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Prefabs[model], spawnLocation, Quaternion.Euler(0f, 0f, 180f));
        spaceship.transform.localScale = new Vector2(parameters.Size, parameters.Size);
        spaceship.GetComponent<Spaceship>().Initialize(null, null, parameters);
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
