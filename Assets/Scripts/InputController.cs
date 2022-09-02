using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TankMovement))]
public class InputController : MonoBehaviour
{
    public UnityEvent<float>[] shootEvents;

    PlayerInput inputComponent;
    TankMovement movement;

    Vector2 shootPoint;

    private void Awake()
    {
        movement = GetComponent<TankMovement>();
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
            case "Shoot 1":
                if (shootEvents.Length > 0) shootEvents[0].Invoke(callbackContext.ReadValue<float>());
                break;
            case "Shoot 2":
                if (shootEvents.Length > 1) shootEvents[1].Invoke(callbackContext.ReadValue<float>());
                break;
            default:
                break;
        }
    }
}
