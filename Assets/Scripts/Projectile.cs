using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float startSpeed;
    [SerializeField] float projectileSize;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float range;
    [SerializeField] AnimationCurve scaleCurve;

    [Space]
    [SerializeField] SpriteShadow shadow;
    [SerializeField] AnimationCurve shadowDistance;

    [Space]
    [SerializeField] GameObject landFX;
    [SerializeField] GameObject impactFX;

    float age;
    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.right * startSpeed;
    }

    private void FixedUpdate()
    {
        float speed = rigidbody.velocity.magnitude;
        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, projectileSize, rigidbody.velocity, speed * Time.deltaTime + 0.01f, collisionMask);
        if (hit)
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                if (damage > 0.001f) health.Damage(new DamageArgs(transform.root.gameObject, damage));
            }

            if (impactFX)
            {
                impactFX.SetActive(true);
                impactFX.transform.SetParent(null);
            }
            Destroy(gameObject);
        }

        age += Time.deltaTime;
        if (age > range / startSpeed)
        {
            if (landFX)
            {
                landFX.SetActive(true);
                landFX.transform.SetParent(null);
            }
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = Vector3.one * scaleCurve.Evaluate(age / range);
            shadow.Offset = Vector2.down * shadowDistance.Evaluate(age / range);
        }
    }
}
