using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    [Space]
    [SerializeField] Transform[] detachOnDeath;

    [Space]
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject deathPrefab;

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
        if (deathPrefab) Instantiate(deathPrefab, transform.position, Quaternion.identity);

        foreach (var detach in detachOnDeath)
        {
            detach.SetParent(null);
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
