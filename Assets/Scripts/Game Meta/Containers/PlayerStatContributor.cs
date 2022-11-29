using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatContributor : MonoBehaviour
{
    public TankGun mainGun;

    float startTime;

    private void OnEnable()
    {
        mainGun.ShootEvent += OnShootEvent;
        mainGun.HitEvent += OnHitEvent;
    }

    private void OnDisable()
    {
        mainGun.ShootEvent -= OnShootEvent;
        mainGun.HitEvent -= OnHitEvent;
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {   
        Stats.SetValue("time_alive", Time.time - startTime);
    }

    private void OnShootEvent()
    {
        Stats.IncrementValue("shots_fired", 1.0f);
    }

    private void OnHitEvent(GameObject hitObject, DamageArgs args)
    {
        Stats.IncrementValue("damage_delt", args.damage);

        if (hitObject.TryGetComponent(out Health health))
        {
            if (health.currentHealth <= 0)
            {
                Stats.IncrementValue("enemies_killed", 1.0f);
            }
        }
    }
}
