using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TankGun : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireDelay;

    [Space]
    [SerializeField] UnityEvent shootEventEditor;

    float nextFireTime;

    public event System.Action OnShootEvent;

    public float FireDelay => fireDelay;
    public float NextFireTime => nextFireTime;

    public void Shoot()
    {
        if (Time.time < nextFireTime) return;

        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        nextFireTime = Time.time + fireDelay;

        OnShootEvent?.Invoke();
        shootEventEditor?.Invoke();
    }
}
