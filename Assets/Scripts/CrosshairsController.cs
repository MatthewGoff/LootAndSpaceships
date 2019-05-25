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

        Spaceship subject = GameManager.Instance.PlayerController;
        Dictionary<int, RadarProfile> radarProfiles = Omniscience.Instance.PingRadar();
        if (subject.HasValidTarget && radarProfiles.ContainsKey(subject.TargetUID))
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
