using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(TankGun))]
public class InputController : MonoBehaviour
{
    PlayerInput inputComponent;
    TankMovement movement;
    TankGun gun;

    Vector2 shootPoint;

    private void Awake()
    {
        movement = GetComponent<TankMovement>();
        gun = GetComponent<TankGun>();
    }

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

    private void Update()
    {
        movement.ShootPoint = Camera.main.ScreenToWorldPoint(shootPoint);
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext)
    {
        switch (callbackContext.action.name)
        {
            case "Throttle":
                movement.ThrottleInput = callbackContext.ReadValue<float>();
                break;
            case "Turning":
                movement.TurnInput = callbackContext.ReadValue<float>();
                break;
            case "Shoot Point":
                shootPoint = callbackContext.ReadValue<Vector2>();
                break;
            case "Shoot":
                gun.Shoot();
                break;
            default:
                break;
        }
    }
}
