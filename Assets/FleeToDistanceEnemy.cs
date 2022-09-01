using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeToDistanceEnemy : EnemyBase
{
    [SerializeField] float fleeDistance;
    [SerializeField] float attackDistance;
    [SerializeField] int attackIndex;

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
                Attack(attackIndex);
                if (dist < fleeDistance)
                {
                    fleeing = true;
                }
            }
        }
    }
}
