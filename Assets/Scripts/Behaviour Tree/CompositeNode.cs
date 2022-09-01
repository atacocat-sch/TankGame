using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : BehaviourNode
{
    public List<CompositeNode> children;
}
