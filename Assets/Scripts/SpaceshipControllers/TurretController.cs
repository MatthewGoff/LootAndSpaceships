﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : Spaceship
{
    public GameObject SpriteColor1;
    public GameObject SpriteColor5;

    protected override void ModelSpecificInitialization()
    {
        transform.localRotation = Quaternion.identity;
        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + Team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        SpriteColor5.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(5);
    }
}
