using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckWallDistance : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
        
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.floorCenter);
        if (_distance > 29.0f)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
