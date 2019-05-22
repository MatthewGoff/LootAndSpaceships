using UnityEngine;

public class Pi2Controller : Spaceship
{
    public GameObject SpriteColor1;

    protected override void ModelSpecificInitialization()
    {
        ColorPalett colorPalett = ConfigurationColorPaletts.Instance.GetColorPalett("Team " + Team);
        SpriteColor1.GetComponent<SpriteRenderer>().color = colorPalett.GetColor(1);
    }
}
