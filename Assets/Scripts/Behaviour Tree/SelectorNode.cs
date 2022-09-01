using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    BehaviourNode pendingNode = null;

    public override BehaviourEvaluationState Evaluate(BehaviourTree context)
    {
        foreach (BehaviourNode child in children)
        {
            if (pendingNode)
            {
                if (pendingNode != child) continue;
                else pendingNode = null;
            }

            switch (child.Evaluate(context))
            {
                case BehaviourEvaluationState.Success:
                    return BehaviourEvaluationState.Success;

                case BehaviourEvaluationState.Pending:
                    pendingNode = child;
                    return BehaviourEvaluationState.Pending;

                default:
                    break;
            }
        }

        return BehaviourEvaluationState.Fail;
    }
}