using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    [SerializeField] Transform leftTrack;
    [SerializeField] Transform rightTrack;
    [SerializeField] Transform turret;

    [Space]
    [SerializeField] float maxTrackForce;
    [SerializeField] float trackSpeedSmoothing;
    [SerializeField] float trackFriction;

    [Space]
    [SerializeField] float turretMaxSpeed;
    [SerializeField] float turretSmoothTime;

    float leftTrackInput;
    float rightTrackInput;
    Vector2 shootPoint;

    float leftTrackSpeed;
    float rightTrackSpeed;
    float turretRotation;

    float leftTrackSpeedVelocity;
    float rightTrackSpeedVelocity;
    float turretRotationVelocity;

    new Rigidbody2D rigidbody;
    PlayerInput inputComponent;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
        RotateTurret();

        UpdateTrack(leftTrack, leftTrackInput, ref leftTrackSpeed, ref leftTrackSpeedVelocity);
        UpdateTrack(rightTrack, rightTrackInput, ref rightTrackSpeed, ref rightTrackSpeedVelocity);
    }

    private void RotateTurret()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(shootPoint) - turret.position;
        float targetRotation = Mathf.Atan2(worldPoint.y, worldPoint.x) * Mathf.Rad2Deg;

        turretRotation = Mathf.SmoothDampAngle(turretRotation, targetRotation, ref turretRotationVelocity, turretSmoothTime, turretMaxSpeed);
        turret.rotation = Quaternion.Euler(0.0f, 0.0f, turretRotation);
    }

    private void UpdateTrack(Transform track, float input, ref float speed, ref float speedVelocity)
    {
        speed = Mathf.SmoothDamp(speed, input * maxTrackForce, ref speedVelocity, trackSpeedSmoothing);

        Vector2 current = rigidbody.GetPointVelocity(track.position);
        Vector2 target = track.right * speed;
        Vector2 force = (target - current) * trackFriction;
        rigidbody.AddForceAtPosition(force, track.position);
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext)
    {
        switch (callbackContext.action.name)
        {
            case "Left Track":
                leftTrackInput = callbackContext.ReadValue<float>();
                break;
            case "Right Track":
                rightTrackInput = callbackContext.ReadValue<float>();
                break;
            case "Shoot Point":
                shootPoint = callbackContext.ReadValue<Vector2>();
                break;
            default:
                break;
        }
    }
}
