using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustEffectController : MonoBehaviour
{

    public GameObject ExhaustGlow;
    public GameObject ExhaustParticles;

    public void Start()
    {
        Disable();
    }

    public void Enable()
    {
        ExhaustGlow.SetActive(true);
        ExhaustParticles.GetComponent<ParticleSystem>().Play();
    }

    public void Disable()
    {
        ExhaustGlow.SetActive(false);
        ExhaustParticles.GetComponent<ParticleSystem>().Stop();
    }
}
