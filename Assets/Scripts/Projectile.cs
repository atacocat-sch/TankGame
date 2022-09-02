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
    Vector2 previousPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.right * startSpeed;
    }

    private void Start()
    {
        previousPosition = rigidbody.position;
    }

    private void FixedUpdate()
    {
        Vector2 point = previousPosition;
        Vector2 vector = rigidbody.position + rigidbody.velocity * Time.deltaTime - previousPosition;
        float distance = vector.magnitude;
        Vector2 direction = vector / distance;

        RaycastHit2D hit = Physics2D.CircleCast(point, projectileSize, direction, distance + 0.1f, collisionMask);
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
            transform.localScale = Vector3.one * scaleCurve.Evaluate(startSpeed * age / range);
            shadow.Offset = Vector2.down * shadowDistance.Evaluate(startSpeed * age / range);
        }

        previousPosition = rigidbody.position;
    }
}
