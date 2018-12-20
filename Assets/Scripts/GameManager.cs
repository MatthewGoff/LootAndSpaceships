using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ITargetable PlayerTarget;
    public ITargetable Player;
    public Queue<ITargetable> TargetQueue;
    public GameObject AutopilotTargetEffect;

    private void Awake()
    {
        Instance = this;
        Prefabs.LoadPrefabs();
    }

    private void Start()
    {
        GameObject player = Instantiate(Prefabs.Player, new Vector2(0f, 0f), Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Initialize("Admiral Ackbar");
        playerController.AutopilotTargetEffect = AutopilotTargetEffect;
        RadarController.Instance.Subject = playerController;
        MasterCameraController.Instance.Subject = playerController;
        Player = playerController;

        GameObject enemy = Instantiate(Prefabs.Enemy, new Vector2(30f, 0f), Quaternion.Euler(0f, 0f, 180f));
        enemy.GetComponent<EnemyController>().Initialize("Enemy 1");

        TargetQueue = new Queue<ITargetable>();
        TargetQueue.Enqueue(enemy.GetComponent<EnemyController>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ITargetable nextTarget = TargetQueue.Dequeue();
            PlayerTarget = nextTarget;
            TargetQueue.Enqueue(nextTarget);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerTarget = null;
        }
    }

    public static bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeTarget(ITargetable target)
    {
        PlayerTarget = target;
    }
}
