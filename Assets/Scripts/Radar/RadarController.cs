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
    /**
     * The radius in pixels of the radar display;
     */
    public float PixelRadius;

    private RadarPip[] Pips;

    private void Awake()
    {
        Instance = this;
        Pips = new RadarPip[10];
        for (int i = 0; i < Pips.Length; i++)
        {
            Pips[i] = new RadarPip(this);
        }
        // Calculate the radius in pixels of the radar display. We multiply
        // the image width by 0.9f because that happens to be the size of the
        // radar screen in the image we're using. And we divide by 2f because
        // we want the radius, not the diameter.
        PixelRadius = GetComponent<RectTransform>().rect.width * 0.9f / 2f;
        UpdateScaleText();
    }

    private void Update()
    {
        Dictionary<int, RadarProfile> profiles = GameManager.Instance.Subject.GetRadarReading();
        ValidatePipPool(profiles.Count);
        int i = 0;
        foreach (int key in profiles.Keys)
        {
            Pips[i++].Show(profiles[key]);
        }
        while (i < Pips.Length)
        {
            Pips[i++].Hide();
        }
    }

    private void ValidatePipPool(int count)
    {
        if (Pips.Length < count)
        {
            RadarPip[] newPips = new RadarPip[Pips.Length * 2];
            for (int i = 0; i < Pips.Length; i++)
            {
                newPips[i] = Pips[i];
            }
            for (int i = Pips.Length; i < newPips.Length; i++)
            {
                newPips[i] = new RadarPip(this);
            }
            Pips = newPips;
            ValidatePipPool(count);
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
}
