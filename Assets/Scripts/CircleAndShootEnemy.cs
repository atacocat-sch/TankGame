using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAndShootEnemy : EnemyBase
{
    public float circleRadius;
    public float attackRadius;
    public float angleOffset;
    public float attackAngle;
    public int circleAttackIndex;

    protected override void Update()
    {
        base.Update();

        if (Target)
        {
            Vector2 vectorBetween = (Target.transform.position - transform.position);
            float distToTarget = vectorBetween.magnitude;
            Vector2 direction = vectorBetween / distToTarget;

            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + angleOffset;
            Vector2 offsetDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;
            MoveTowards((Vector2)Target.transform.position + offsetDirection);

            Debug.DrawLine(Target.transform.position, (Vector2)Target.transform.position + offsetDirection, Color.blue);

            if (distToTarget < attackRadius)
            {
                float dot = Vector2.Dot(direction, MuzzleDirection);
                if (Mathf.Acos(dot) * Mathf.Rad2Deg < attackAngle)
                {
                    Attack(circleAttackIndex);
                }
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.white;
    }
}
