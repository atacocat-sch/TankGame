using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float startSpeed;
    [SerializeField] float projectileSize;
    [SerializeField] float range;
    [SerializeField] AnimationCurve scaleCurve;

    [Space]
    [SerializeField] SpriteShadow shadow;
    [SerializeField] AnimationCurve shadowDistance;

    [Space]
    [SerializeField] GameObject landFX;

    float distanceTraveled;
    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.right * startSpeed;
    }

    private void FixedUpdate()
    {
        float speed = rigidbody.velocity.magnitude;
        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, projectileSize, rigidbody.velocity, speed * Time.deltaTime + 0.01f);
        if (hit)
        {
            Destroy(gameObject);
        }

        distanceTraveled += speed * Time.deltaTime;
        if (distanceTraveled > range)
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
            transform.localScale = Vector3.one * scaleCurve.Evaluate(distanceTraveled / range);
            shadow.Offset = Vector2.down * shadowDistance.Evaluate(distanceTraveled / range);
        }
    }
}
