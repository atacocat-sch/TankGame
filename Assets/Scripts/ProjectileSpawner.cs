using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] UnityEvent fireEvent;

    public void Shoot ()
    {
        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        fireEvent?.Invoke();
    }
}
