using UnityEngine;

public class ExpBarController : MonoBehaviour
{

    void Update()
    {
        Vector3 scale = transform.localScale;
        if (GameManager.Instance.PlayerAlive)
        {
            int currentExp = GameManager.Instance.Player.Experience;
            int nextLevelExp = Configuration.ExpForLevel(GameManager.Instance.Player.Level + 1);
            scale.x = (float)currentExp / (float)nextLevelExp;
        }
        else
        {
            scale.x = 0f;
        }
        transform.localScale = scale;
    }
}
