using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemyController PlayerTarget;
    public Queue<EnemyController> TargetQueue;
    public GameObject AutopilotTargetEffect;

    private int EnemyCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RadarOmniscience.Initialize();

        GameObject player = Instantiate(Prefabs.Instance.Player, new Vector2(0f, 0f), Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Initialize("Player 1 ");
        playerController.AutopilotTargetEffect = AutopilotTargetEffect;
        RadarController.Instance.Subject = playerController;
        MasterCameraController.Instance.Subject = playerController;
        BottomHUDController.Instance.Subject = playerController;

        TargetQueue = new Queue<EnemyController>();
        EnemyCounter = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (TargetQueue.Count != 0)
            {
                EnemyController nextTarget = TargetQueue.Dequeue();
                PlayerTarget = nextTarget;
                TargetQueue.Enqueue(nextTarget);
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerTarget = null;
        }
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

        TargetQueue.Enqueue(enemy.GetComponent<EnemyController>());
    }

    public static bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeTarget(EnemyController target)
    {
        PlayerTarget = target;
    }
}
