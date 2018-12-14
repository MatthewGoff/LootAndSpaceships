using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ITargetable PlayerTarget;
    public GameObject[] Enemies;
    public Queue<ITargetable> TargetQueue;

    private void Awake()
    {
        Instance = this;
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

}
