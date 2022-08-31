using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankGun : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] bool loaded;
    [SerializeField] bool locked;

    PlayerInput inputComponent;

    public event System.Action OnShootEvent;

    public bool Locked { get => locked; set => locked = value; }
    public bool Loaded { get => loaded; set => loaded = value; }

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
                Shoot();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        if (!locked || !loaded) return;

        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        loaded = false;

        OnShootEvent?.Invoke();
    }
}
