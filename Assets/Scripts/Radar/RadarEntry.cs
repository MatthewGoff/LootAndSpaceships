﻿using UnityEngine;

public class RadarEntry
{
    private static readonly GameObject PIP_PREFAB = (GameObject)Resources.Load("Prefabs/Pip");
    private static readonly GameObject ARROW_PREFAB = (GameObject)Resources.Load("Prefabs/Arrow");

    public RadarTarget Target { get; private set; }
    private GameObject Pip;
    private GameObject Arrow;
    private RadarController RadarController;

    public RadarEntry(RadarController radarController, RadarTarget target)
    {
        RadarController = radarController;
        Target = target;
        Pip = GameObject.Instantiate(PIP_PREFAB, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Pip.transform.SetParent(RadarController.ContentsTransform.transform);
        Arrow = GameObject.Instantiate(ARROW_PREFAB, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Arrow.transform.SetParent(RadarController.ContentsTransform.transform);
    }

    public void Update()
    {
        Vector2 playerPosition = GameManager.Instance.GetPlayerPosition();
        Vector2 relativePosition = Target.GetPosition() - playerPosition;
        Vector2 radarPosition = relativePosition * (RadarController.PixelRadius / 3f) / RadarController.RadarScale;
        if (radarPosition.magnitude > RadarController.PixelRadius)
        {
            Pip.SetActive(false);
            Arrow.SetActive(true);
            radarPosition = radarPosition * RadarController.PixelRadius / radarPosition.magnitude;
            Arrow.transform.localPosition = radarPosition;
            Arrow.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.right, radarPosition));
        }
        else
        {
            Pip.SetActive(true);
            Arrow.SetActive(false);
            Pip.transform.localPosition = radarPosition;
        }
    }

    public bool MyTarget(RadarTarget target)
    {
        return Target == target;
    }

    public void Destroy()
    {
        GameObject.Destroy(Pip);
        GameObject.Destroy(Arrow);
    }
}
