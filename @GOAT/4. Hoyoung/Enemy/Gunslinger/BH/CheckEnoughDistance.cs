using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckEnoughDistance : ActionNode
{
    protected override void OnStart() 
    {
        
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if ((context.rifleDistance < _distance)&&(Random.Range(0,10)>=3))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
