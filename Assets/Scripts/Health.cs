using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    [Space]
    public Transform[] detachOnDeath;

    [Space]
    public GameObject damagePrefab;
    public GameObject deathPrefab;

    public float CurrentHeath => currentHealth;
    public float MaxHealth => maxHealth;

    public void Damage (DamageArgs args)
    {
        currentHealth -= args.damage;

        if (damagePrefab) Instantiate(damagePrefab, transform.position, Quaternion.identity);

        if (currentHealth < 0.0f)
        {
            Die(args);
        }
    }

    public void Die(DamageArgs args)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (deathPrefab)
        {
            GameObject deathInstance = Instantiate(deathPrefab, transform.position, Quaternion.identity);
            if (rigidbody && deathInstance.TryGetComponent(out Rigidbody2D diRigidbody))
            {
                diRigidbody.velocity = rigidbody.velocity;
            }
        }

        foreach (var detach in detachOnDeath)
        {
            detach.SetParent(null);
            if (rigidbody && detach.TryGetComponent(out Rigidbody2D detachRigidbody))
            {
                detachRigidbody.velocity = rigidbody.velocity;
            }
        }

        gameObject.SetActive(false);
    }

    public void Regen (DamageArgs args)
    {
        currentHealth = Mathf.Min(currentHealth + args.damage, MaxHealth);
    }
}

public struct DamageArgs
{
    public GameObject damager;
    public float damage;

    public DamageArgs(GameObject damager, float damage)
    {
        this.damager = damager;
        this.damage = damage;
    }
}
