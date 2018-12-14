using System.Collections;
using UnityEngine;

public class EnemyController : CombatantManager
{
    public GameObject FDNCanvas;
    public GameObject FireEffect;

    private FDNCanvasController FDNCanvasController;
    private float BurnDuration = 3f;
    private float BurnEndTime;
    private bool Burning;

    private void Awake()
    {
        Team = 1;
        FDNCanvasController = FDNCanvas.GetComponent<FDNCanvasController>();
    }

    public override Vector2 GetPosition()
    {
        return transform.position;
    }

    public override void RecieveHit(int damage, DamageType damageType)
    {
        FDNCanvasController.Display(damage, damage/100f);
        if (damageType == DamageType.Explosion)
        {
            BurnEndTime = Time.time + BurnDuration;
            if (!Burning)
            {
                StartCoroutine("Burn");
            }
        }
    }

    private IEnumerator Burn()
    {
        Burning = true;
        FireEffect.SetActive(true);

        while(Time.time < BurnEndTime)
        {
            RecieveHit(Mathf.RoundToInt(Random.Range(0f, 10f)), DamageType.Fire);
            yield return new WaitForSeconds(0.1f);
        }

        Burning = false;
        FireEffect.SetActive(false);
    }
}
