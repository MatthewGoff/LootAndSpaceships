using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AutopilotTargetEffect;
    public PlayerController Subject;

    private int EnemyCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RadarOmniscience.Initialize();
        SpaceshipRegistry.Initialize();

        GameObject player = Instantiate(Prefabs.Instance.Player, new Vector2(0f, 0f), Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Initialize("Player 1");
        playerController.AutopilotTargetEffect = AutopilotTargetEffect;
        Subject = playerController;

        EnemyCounter = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        EnemyCounter++;

        GameObject enemy = Instantiate(Prefabs.Instance.Enemy, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
        enemy.GetComponent<EnemyController>().Initialize("Enemy "+EnemyCounter.ToString());
    }

    public static bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void SelectTarget(int uid)
    {
        Subject.SelectTarget(uid);
    }

    public void PlayerDeath()
    {

    }
}
