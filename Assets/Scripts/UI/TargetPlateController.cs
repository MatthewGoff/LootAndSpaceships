using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPlateController : MonoBehaviour
{
    public GameObject ContentHolder;
    public GameObject NameText;
    public GameObject NameTextHighlight;
    public GameObject HealthBar;
    public GameObject ShieldBar;
    public GameObject EnergyBar;
    public GameObject FuelBar;

    private bool HasTarget;
    private Spaceship Target;

    void Update()
    {
        if (!GameManager.Instance.PlayerAlive)
        {
            HasTarget = false;
            ContentHolder.SetActive(false);
            return;
        }

        Spaceship subject = GameManager.Instance.PlayerController;
        Spaceship newTarget = Omniscience.Instance.GetSpaceship(subject.TargetUID);
        if (subject.HasTarget && newTarget != null)
        {
            if (!HasTarget || Target != newTarget)
            {
                if (HasTarget)
                {
                    Target.PortraitCamera.SetActive(false);
                }
                Target = newTarget;
                HasTarget = true;
                Target.PortraitCamera.SetActive(true);

                NameText.GetComponent<Text>().text = Target.Name;
                NameTextHighlight.GetComponent<Text>().text = Target.Name;

                ContentHolder.SetActive(true);
            }

            RadarProfile profile = Target.GetRadarProfile();
            HealthBar.transform.localScale = new Vector2(1, profile.CurrentHealth / profile.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, profile.CurrentShield / profile.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, profile.CurrentEnergy / profile.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, profile.CurrentFuel / profile.MaxFuel);
        }
        else
        {
            HasTarget = false;
            ContentHolder.SetActive(false);
        }
    }
}
