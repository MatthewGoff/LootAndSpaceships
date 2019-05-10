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
        PlayerController subject = GameManager.Instance.Subject;
        Dictionary<int, RadarProfile> radarProfiles = RadarOmniscience.Instance.PingRadar();
        if (subject.HasTarget && radarProfiles.ContainsKey(subject.TargetUID))
        {
            SpriteRenderer.enabled = true;
            transform.position = subject.GetRadarReading()[subject.TargetUID].Position;
            float angle = (transform.eulerAngles.z + TurnRate * Time.deltaTime) % 360;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            SpriteRenderer.enabled = false;
        }
    }
}
