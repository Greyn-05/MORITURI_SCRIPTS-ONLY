using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class VigilanceMove : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
    }

    protected override State OnUpdate() 
    {
        if (context.FailTrigger == true)
        {
            context.FailTrigger = false;
            return State.Failure;
        }
        context.FocusOnTarget(3f);
        context.Move_walk(new Vector3(-1.0f, 0.0f, 0.2f), 0.35f);
        return State.Success;
    }

}
