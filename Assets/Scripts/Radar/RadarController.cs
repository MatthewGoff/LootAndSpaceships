using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{
    public static RadarController Instance;

    private static readonly int MAXIMUM_SCALE = 80;
    private static readonly int MINIMUM_SCALE = 5;

    public GameObject ScaleText1;
    public GameObject ScaleText2;
    public GameObject ContentsTransform;

    public int RadarScale = 10;
    public float PixelRadius;
    public ITargetable Subject;

    private List<RadarPip> RadarEntries;

    private void Awake()
    {
        Instance = this;
        RadarEntries = new List<RadarPip>();
        PixelRadius = GetComponent<RectTransform>().rect.width * 0.9f / 2f;
        UpdateScaleText();
    }

    private void Update()
    {
        foreach (RadarPip radarEntry in RadarEntries)
        {
            radarEntry.Update();
        }   
    }

    public void PlusClicked()
    {
        RadarScale /= 2;
        RadarScale = Mathf.Clamp(RadarScale, MINIMUM_SCALE, MAXIMUM_SCALE);
        UpdateScaleText();
    }

    public void MinusClicked()
    {
        RadarScale *= 2;
        RadarScale = Mathf.Clamp(RadarScale, MINIMUM_SCALE, MAXIMUM_SCALE);
        UpdateScaleText();
    }

    private void UpdateScaleText()
    {
        ScaleText1.GetComponent<Text>().text = RadarScale.ToString();
        ScaleText2.GetComponent<Text>().text = RadarScale.ToString();
    }

    public void AddToRadar(RadarTarget radarTarget)
    {
        RadarEntries.Add(new RadarPip(this, radarTarget));
    }

    public void RemoveFromRadar(RadarTarget radarTarget)
    {
        RadarPip toRemove = null;
        foreach (RadarPip entry in RadarEntries)
        {
            if (entry.MyTarget(radarTarget))
            {
                toRemove = entry;
            }
        }
        if (toRemove != null)
        {
            toRemove.Destroy();
            RadarEntries.Remove(toRemove);
        }
    }
}
