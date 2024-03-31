using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsTooFar : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if(_distance > 10.0f)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
