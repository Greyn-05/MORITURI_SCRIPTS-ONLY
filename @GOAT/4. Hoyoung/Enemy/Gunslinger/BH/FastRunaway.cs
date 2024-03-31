using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FastRunaway : ActionNode
{
    protected override void OnStart()
    {
        context._myState = BattleState.Move;
    }

    protected override void OnStop()
    {
        context._myState = BattleState.Move;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if(_distance > context.rifleDistance + 1.0f)
        {
            return State.Running;
        }
        return State.Success;
    }
}
