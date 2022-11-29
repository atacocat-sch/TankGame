using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
public class PlayerDamageFX : MonoBehaviour
{
    public float timeSlowDuration;
    public AnimationCurve timeSlowFactor;

    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.DamageEvent += OnDamageEvent;
    }

    private void OnDisable()
    {
        health.DamageEvent -= OnDamageEvent;
    }

    private void OnDamageEvent(DamageArgs args)
    {
        if (health.currentHealth <= 0) return;

        StartCoroutine(TimeSlow());
    }

    private IEnumerator TimeSlow()
    {
        float percent = 0.0f;
        while (percent < 1.0f)
        {
            Time.timeScale = timeSlowFactor.Evaluate(percent);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            percent += Time.unscaledDeltaTime / timeSlowDuration;
            yield return null;
        }

        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1.0f;
    }
}
