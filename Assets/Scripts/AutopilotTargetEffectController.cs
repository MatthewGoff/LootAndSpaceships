using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutopilotTargetEffectController : MonoBehaviour
{
    ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        TurnOff();
    }

    private void Update()
    {
        if (GameManager.Instance.PlayerAlive)
        {
            Spaceship player = GameManager.Instance.PlayerController;
            if (player.Autopilot.OnStandby)
            {
                TurnOff();
            }
            else
            {
                TurnOn(player.Autopilot.Target);
            }
        }
        else
        {
            TurnOff();
        }
    }

    private void TurnOn(Vector2 position)
    {
        transform.position = position;
        ParticleSystem.Play();
    }

    private void TurnOff()
    {
        ParticleSystem.Stop();
        ParticleSystem.Clear();
    }
}
