using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ITargetable PlayerTarget;
    public GameObject[] Enemies;
    public Queue<ITargetable> TargetQueue;
    public GameObject Player;

    private void Awake()
    {
        Instance = this;
        Prefabs.LoadPrefabs();
        InitializeTargetQueue();
    }

    private void InitializeTargetQueue()
    {
        TargetQueue = new Queue<ITargetable>();
        foreach (GameObject enemy in Enemies)
        {
            TargetQueue.Enqueue(enemy.GetComponent<EnemyController>());
        }
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

    public Vector2 GetPlayerPosition()
    {
        return Player.GetComponent<PlayerController>().GetPosition();
    }
}
