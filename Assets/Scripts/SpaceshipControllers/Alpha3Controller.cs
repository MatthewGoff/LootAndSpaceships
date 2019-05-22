using UnityEngine;

public class Alpha3Controller : Spaceship
{
    public GameObject SpriteColor1;
    public GameObject SpriteColor5;

    protected override void ModelSpecificInitialization()
    {
        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + Team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
        SpriteColor5.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(5);
    }
}
