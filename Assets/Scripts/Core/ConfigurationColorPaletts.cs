using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationColorPaletts : MonoBehaviour
{
    public static ConfigurationColorPaletts Instance;

    private void Awake()
    {
        Instance = this;
    }

    public ColorPalett GetColorPalett(string name)
    {
        return transform.Find(name).GetComponent<ColorPalett>();
    }
}
