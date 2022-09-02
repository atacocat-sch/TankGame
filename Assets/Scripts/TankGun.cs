using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TankGun : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public Transform muzzle;
    public float fireDelay;
    public bool fullAuto;

    [Space]
    public UnityEvent shootEventEditor;

    float nextFireTime;
    bool triggerState;

    public event System.Action OnShootEvent;

    public float FireDelay => fireDelay;
    public float NextFireTime => nextFireTime;
    public float Cooldown => 1.0f - Mathf.Clamp01((nextFireTime - Time.time) / fireDelay);

    private void Update()
    {
        if (Time.time < nextFireTime) return;
        if (!triggerState) return;

        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        nextFireTime = Time.time + fireDelay;

        OnShootEvent?.Invoke();
        shootEventEditor?.Invoke();

        if (!fullAuto) triggerState = false;
    }

    public void Shoot(float inputValue)
    {
        triggerState = inputValue > 0.5f;
    }
}
