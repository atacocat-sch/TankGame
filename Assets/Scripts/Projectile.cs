using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float hitForce;
    public float startSpeed;
    public float projectileSize;
    public LayerMask collisionMask;
    public float range;
    public AnimationCurve scaleCurve;

    [Space]
    public SpriteShadow shadow;
    public AnimationCurve shadowDistance;

    [Space]
    public GameObject landFX;
    public GameObject impactFX;

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

            if (hit.rigidbody)
            {
                hit.rigidbody.velocity += direction * hitForce;
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
