using System.Collections.Generic;
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
        if (!GameManager.Instance.PlayerAlive)
        {
            SpriteRenderer.enabled = false;
            return;
        }

        DirectedPlayerController subject = GameManager.Instance.PlayerController;
        Dictionary<int, RadarProfile> radarProfiles = RadarOmniscience.Instance.PingRadar();
        if (subject.HasTarget && radarProfiles.ContainsKey(subject.TargetUID))
        {
            SpriteRenderer.enabled = true;
            transform.position = radarProfiles[subject.TargetUID].Position;
            float angle = (transform.eulerAngles.z + TurnRate * Time.deltaTime) % 360;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            SpriteRenderer.enabled = false;
        }
    }
}
