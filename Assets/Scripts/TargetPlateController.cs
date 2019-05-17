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

        DirectedPlayerController subject = GameManager.Instance.PlayerController;
        Dictionary<int, Spaceship> spaceships = SpaceshipRegistry.Instance.Spaceships;
        if (subject.HasTarget && spaceships.ContainsKey(subject.TargetUID))
        {
            Spaceship newTarget = spaceships[subject.TargetUID];
            if (!HasTarget ||  Target != newTarget)
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

            HealthBar.transform.localScale = new Vector2(1, Target.CurrentHealth / Target.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, Target.CurrentShield / Target.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, Target.CurrentEnergy / Target.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, Target.CurrentFuel / Target.MaxFuel);
        }
        else
        {
            HasTarget = false;
            ContentHolder.SetActive(false);
        }
    }
}
