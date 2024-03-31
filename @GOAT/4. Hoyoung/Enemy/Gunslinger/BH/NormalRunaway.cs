using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class NormalRunaway : ActionNode
{
    private Vector3[] rocalDir = new Vector3[] {new Vector3(-0.2f, 0.0f, -1.0f), new Vector3(0.2f, 0.0f, -1.0f), new Vector3(0.0f, 0.0f, -1.0f) };
    private Vector3 inputDir;
    private float maintainTime;
    private float elapsedTime;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        inputDir = rocalDir[Random.Range(0, rocalDir.Length)];
        maintainTime = Random.Range(0.8f, 1.2f);
        elapsedTime = 0f;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    protected override State OnUpdate() 
    {
        if (!context.CheckRunningAnimation("Ground"))
        {
            return State.Running;
        }
        elapsedTime += Time.deltaTime;
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if ((_distance < context.shotgunDistance)&&(elapsedTime < maintainTime))
        {
            context.FocusOnTarget(10f);
            context.Move_walk(inputDir, (5.5f + 1*(context.enemyPhase-1)));
            return State.Running;
        }
        return State.Success;
    }
}
