using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ParticlesInheritRotation : MonoBehaviour
{
    new ParticleSystem particleSystem;

    private void Awake()
    {
    }

    private void Update()
    {
        if (!particleSystem) particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem.main;

        main.startRotationX = -transform.eulerAngles.x * Mathf.Deg2Rad;
        main.startRotationY = -transform.eulerAngles.y * Mathf.Deg2Rad;
        main.startRotationZ = -transform.eulerAngles.z * Mathf.Deg2Rad;
    }
}
