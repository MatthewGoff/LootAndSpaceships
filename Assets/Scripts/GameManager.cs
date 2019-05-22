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
        if (PlayerParadigm == 1)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Alpha1");
            parameters.Name = "Player 1";
            parameters.Team = 0;
            parameters.AIType = AIType.Player;
            PlayerController = spaceship.GetComponent<Spaceship>();
            PlayerController.Initialize(null, parameters);
        }
        else if (PlayerParadigm == 2)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(0f, 0f), Quaternion.identity);
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Alpha1");
            parameters.Name = "Player 1";
            parameters.Team = 0;
            parameters.AIType = AIType.Player;
            parameters.TargetingType = TargetingType.Unbound;
            PlayerController = spaceship.GetComponent<Spaceship>();
            PlayerController.Initialize(null, parameters);
        }
        else if (PlayerParadigm == 3)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1Omni, new Vector2(0f, 0f), Quaternion.identity);
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Alpha1Omni");
            parameters.Name = "Player 1";
            parameters.Team = 0;
            parameters.AIType = AIType.Player;
            parameters.TargetingType = TargetingType.Unbound;
            PlayerController = spaceship.GetComponent<Spaceship>();
            PlayerController.Initialize(null, parameters);
        }
    }

    private void SpawnEnemies(int level)
    {
        if (level == 1)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Alpha1");
            parameters.Name = "Enemy 1 [Alpha 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 2)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha2, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Alpha2");
            parameters.Name = "Enemy 1 [Alpha 2]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha3, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Alpha3");
            parameters.Name = "Enemy 2 [Alpha 3]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha3, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Alpha3");
            parameters.Name = "Enemy 3 [Alpha 3]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 3)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Beta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Beta1");
            parameters.Name = "Enemy 1 [Beta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha4, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Alpha4");
            parameters.Name = "Enemy 2 [Alpha 4]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Alpha4, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Alpha4");
            parameters.Name = "Enemy 3 [Alpha 4]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 4)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Eta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Eta1");
            parameters.Name = "Enemy 1 [Eta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Delta1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Delta1");
            parameters.Name = "Enemy 2 [Delta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Delta1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Delta1");
            parameters.Name = "Enemy 3 [Delta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 5)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Gamma1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Gamma1");
            parameters.Name = "Enemy 1 [Gamma 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Iota1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Iota1");
            parameters.Name = "Enemy 2 [Iota 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Iota1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Iota1");
            parameters.Name = "Enemy 3 [Iota 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 6)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Omicron1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Omicron1");
            parameters.Name = "Enemy 1 [Omicron 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Nu1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Nu1");
            parameters.Name = "Enemy 2 [Nu 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Nu1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Nu1");
            parameters.Name = "Enemy 3 [Nu 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 7)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Phi1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Phi1");
            parameters.Name = "Enemy 1 [Phi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Pi1");
            parameters.Name = "Enemy 2 [Pi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Pi1");
            parameters.Name = "Enemy 3 [Pi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 8)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Pi2, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Pi2");
            parameters.Name = "Enemy 1 [Pi 2]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Sigma1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Sigma1");
            parameters.Name = "Enemy 2 [Sigma 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Sigma1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Sigma1");
            parameters.Name = "Enemy 3 [Sigma 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 9)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Theta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Theta1");
            parameters.Name = "Enemy 1 [Theta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Tau1, new Vector2(30f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Tau1");
            parameters.Name = "Enemy 2 [Tau 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Tau1, new Vector2(30f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Tau1");
            parameters.Name = "Enemy 3 [Tau 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
        }
        else if (level == 10)
        {
            GameObject spaceship = Instantiate(SpaceshipPrefabs.Instance.Upsilon1, new Vector2(50f, 0f), Quaternion.Euler(0f, 0f, 180f));
            SpaceshipParameters parameters = SpaceshipTable.GetModelParameters("Upsilon1");
            parameters.Name = "Enemy 1 [Upsilon 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(40f, 0f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Xi1");
            parameters.Name = "Enemy 2 [Xi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(50f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Xi1");
            parameters.Name = "Enemy 3 [Xi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Xi1, new Vector2(50f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Xi1");
            parameters.Name = "Enemy 4 [Xi 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Zeta1");
            parameters.Name = "Enemy 5 [Zeta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(40f, -5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Zeta1");
            parameters.Name = "Enemy 6 [Zeta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(40f, 5f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Zeta1");
            parameters.Name = "Enemy 7 [Zeta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(50f, -10f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Zeta1");
            parameters.Name = "Enemy 8 [Zeta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);

            spaceship = Instantiate(SpaceshipPrefabs.Instance.Zeta1, new Vector2(50f, 10f), Quaternion.Euler(0f, 0f, 180f));
            parameters = SpaceshipTable.GetModelParameters("Zeta1");
            parameters.Name = "Enemy 9 [Zeta 1]";
            parameters.Team = 1;
            spaceship.GetComponent<Spaceship>().Initialize(null, parameters);
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
