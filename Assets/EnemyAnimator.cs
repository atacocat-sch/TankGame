using UnityEngine;

[DisallowMultipleComponent]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] targets;
    [SerializeField] float movementTilt;

    EnemyBase enemyBase;
    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    private void Update()
    {
        bool flip = enemyBase ? enemyBase.MoveDirection.x > 0.0f : false;
        float rotation = rigidbody ? rigidbody.velocity.x * movementTilt : 0.0f;

        foreach (SpriteRenderer target in targets)
        {
            target.flipX = flip;
            target.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation);
        }
    }
}
