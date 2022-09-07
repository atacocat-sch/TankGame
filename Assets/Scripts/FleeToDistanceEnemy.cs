using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeToDistanceEnemy : EnemyBase
{
    public float fleeDistance;
    public float attackDistance;
    public int attackIndex;

    bool fleeing = false;

    protected override void Update()
    {
        base.Update();

        if (Target)
        {
            Vector2 vector = Target.transform.position - transform.position;
            float dist = vector.magnitude;

            if (fleeing)
            {
                MoveInDirection(-vector);

                if (dist > attackDistance)
                {
                    fleeing = false;
                }
            }
            else
            {
                MoveInDirection(Vector2.zero);

                Attack(attackIndex);
                if (dist < fleeDistance)
                {
                    fleeing = true;
                }
            }
        }
    }
}
