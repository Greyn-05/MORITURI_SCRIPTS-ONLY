using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class GuardWhenClose : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
        context.animationController.guard = false;
    }

    protected override State OnUpdate() 
    {
        if(context.FailTrigger == true)
        {
            context.FailTrigger = false;
            return State.Failure;
        }
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if(_distance < 2.8f)
        {
            context.FocusOnTarget(3.0f);
            context._myState = BattleState.Guard;
            context.animationController.guard = true;
            return State.Running;
        }
        return State.Failure;
    }
}
