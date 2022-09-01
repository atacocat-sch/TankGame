using System.Collections;
using UnityEngine;

public abstract class BehaviourNode : ScriptableObject
{
    public abstract BehaviourEvaluationState Evaluate(BehaviourTree context);
}

public enum BehaviourEvaluationState
{
    Fail = 0,
    Success = 1,
    Pending = 2,
}
