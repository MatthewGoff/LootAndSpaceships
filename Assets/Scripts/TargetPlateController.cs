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

    private EnemyController Target;

    void Update()
    {
        if (GameManager.Instance.PlayerTarget == null)
        {
            Target = null;
            ContentHolder.SetActive(false);
        }
        else
        {
            EnemyController newTarget = GameManager.Instance.PlayerTarget;
            if (newTarget != Target)
            {
                if (Target != null)
                {
                    Target.PortraitCamera.SetActive(false);
                }
                newTarget.PortraitCamera.SetActive(true);
                Target = newTarget;

                NameText.GetComponent<Text>().text = Target.Name;
                NameTextHighlight.GetComponent<Text>().text = Target.Name;

                ContentHolder.SetActive(true);
            }


            HealthBar.transform.localScale = new Vector2(1, Target.CurrentHealth / Target.MaxHealth);
            ShieldBar.transform.localScale = new Vector2(1, Target.CurrentShield / Target.MaxShield);
            EnergyBar.transform.localScale = new Vector2(1, Target.CurrentEnergy / Target.MaxEnergy);
            FuelBar.transform.localScale = new Vector2(1, Target.CurrentFuel / Target.MaxFuel);
        }
    }
}
