using UnityEngine;

public class CrosshairsController : MonoBehaviour {

    private static readonly float TurnRate = 30f;
    private SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        ITargetable target = GameManager.Instance.PlayerTarget;
        if (target == null)
        {
            SpriteRenderer.enabled = false;
        }
        else
        {
            SpriteRenderer.enabled = true;
            transform.position = target.GetPosition();
            float angle = (transform.eulerAngles.z + TurnRate * Time.deltaTime) % 360;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
