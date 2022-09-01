using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : BehaviourNode
{
    public override BehaviourEvaluationState Evaluate(BehaviourTree context)
    {
        return BehaviourEvaluationState.Success;
    }
}
