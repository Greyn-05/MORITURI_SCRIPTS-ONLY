using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AfterPistolAttack : ActionNode
{
    private Vector3[] rocalDir = new Vector3[] { new Vector3(-0.4f, 0.0f, -1.0f), new Vector3(0.4f, 0.0f, -1.0f) };
    private Vector3 inputDir;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        inputDir = rocalDir[Random.Range(0, rocalDir.Length)];
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if (_distance < context.shotgunDistance + 1.0f)
        {
            context.FocusOnTarget(10f);
            context.Move_walk(inputDir, 7.0f);
            return State.Running;
        }
        return State.Success;
    }
}
