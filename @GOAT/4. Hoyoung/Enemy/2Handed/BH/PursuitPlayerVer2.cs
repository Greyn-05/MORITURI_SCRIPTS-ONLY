using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PursuitPlayerVer2 : ActionNode
{
    private float pursuitSpeed;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        if(context.enemyPhase == 3)
        {
            pursuitSpeed = 10.0f;
        }
        else
        {
            pursuitSpeed = 5.5f;
        }
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    protected override State OnUpdate() 
    {
        AnimatorStateInfo currentInfo = context.animationController.animator.GetCurrentAnimatorStateInfo(0);
        if(!currentInfo.IsName("Ground"))
        {
            return State.Failure;
        }

        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if (_distance > 2.5f)
        {
            context.FocusOnTarget(6f);
            context.Move(new Vector3(0.0f, 0.0f, 1.0f), pursuitSpeed);
            return State.Running;
        }

        return State.Success;
    }
}
