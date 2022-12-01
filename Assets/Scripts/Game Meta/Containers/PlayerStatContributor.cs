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
        Stats.Main.TimeAlive = Time.time - startTime;
    }

    private void OnShootEvent()
    {
        Stats.Main.ShotsFired++;
    }

    private void OnHitEvent(GameObject hitObject, DamageArgs args)
    {
        Stats.Main.DamageDelt += args.damage;

        if (hitObject.TryGetComponent(out Health health))
        {
            if (health.currentHealth <= 0)
            {
                Stats.Main.TanksDestroyed++;
            }
        }
    }
}
