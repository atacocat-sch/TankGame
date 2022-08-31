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

    PlayerInput inputComponent;
    float nextFireTime;

    public event System.Action OnShootEvent;

    public float FireDelay => fireDelay;
    public float NextFireTime => nextFireTime;

    private void OnEnable()
    {
        if (TryGetComponent(out inputComponent))
        {
            inputComponent.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        if (inputComponent)
        {
            inputComponent.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext)
    {
        switch (callbackContext.action.name)
        {
            case "Shoot":
                if (callbackContext.ReadValue<float>() > 0.1f) Shoot();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime) return;

        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        nextFireTime = Time.time + fireDelay;

        OnShootEvent?.Invoke();
        shootEventEditor?.Invoke();
    }
}
