using UnityEngine;
using UnityEngine.UI;

public class ManifestHUDController : MonoBehaviour
{
    public GameObject CreditsText;
    public GameObject ScrapNumerator;
    public GameObject ScrapDenominator;
    public GameObject BulletText;
    public GameObject RocketText;
    public GameObject MineText;
    public GameObject DroneText;
    public GameObject TurretText;

    private void Update()
    {
        if (!GameManager.Instance.PlayerAlive)
        {
            CreditsText.GetComponent<Text>().text = "0";
            ScrapNumerator.GetComponent<Text>().text = "0";
            ScrapDenominator.GetComponent<Text>().text = "0";
            BulletText.GetComponent<Text>().text = "0";
            RocketText.GetComponent<Text>().text = "0";
            MineText.GetComponent<Text>().text = "0";
            DroneText.GetComponent<Text>().text = "0";
            TurretText.GetComponent<Text>().text = "0";
        }
        else
        {
            Spaceship subject = GameManager.Instance.PlayerController;

            CreditsText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Credits, 5);
            ScrapNumerator.GetComponent<Text>().text = Helpers.FormatNumber(subject.Scrap, 5);
            ScrapDenominator.GetComponent<Text>().text = Helpers.FormatNumber(subject.MaxHullSpace, 5);
            BulletText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Bullets, 5);
            RocketText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Rockets, 5);
            MineText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Mines, 5);
            DroneText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Drones, 5);
            TurretText.GetComponent<Text>().text = Helpers.FormatNumber(subject.Turrets, 5);
        }
    }
}
