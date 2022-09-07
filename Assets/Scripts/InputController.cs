using System;
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
    public float angleScale;

    public UnityEvent<float>[] shootEvents;

    PlayerInput inputComponent;
    TankMovement movement;

    Vector2 shootPoint;
    bool screenSpaceShootPoint;

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
        if (screenSpaceShootPoint) movement.ShootPoint = Camera.main.ScreenToWorldPoint(shootPoint);
        else movement.ShootPoint = shootPoint;
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext)
    {
        if (Application.platform == RuntimePlatform.Android) return;

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
                screenSpaceShootPoint = true;
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

    public void SetMoveDirection (Vector2 direction)
    {
        direction = Vector2.ClampMagnitude(direction, 1.0f);

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        float dot = Vector2.Dot(direction, movement.transform.right);

        movement.ThrottleInput = dot;
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f) * direction.magnitude;
    }

    public void SetShootDirection (Vector2 direction)
    {
        shootPoint = (Vector2)transform.position + direction;
        screenSpaceShootPoint = false;
    }

    public void ShootPress ()
    {
        shootEvents[1].Invoke(1.0f);
    }

    public void ShootRelease()
    {
        shootEvents[1].Invoke(0.0f);
        StartCoroutine(ShootCannonMobile());
    }

    private IEnumerator ShootCannonMobile()
    {
        shootEvents[0].Invoke(1.0f);
        yield return null;
        yield return null;
        shootEvents[0].Invoke(0.0f);
    }
}
