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
    }

    private void OnDisable()
    {
        mainGun.ShootEvent -= OnShootEvent;
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {   
        Stats.SetValue("time_alive", Time.time - startTime);
    }

    private void OnShootEvent()
    {
        Stats.IncrementValue("shots_fired", 1.0f);
    }
}
