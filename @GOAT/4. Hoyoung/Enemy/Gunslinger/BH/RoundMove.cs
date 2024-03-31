using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RoundMove : ActionNode
{
    private Vector3[] rocalDir = new Vector3[] { new Vector3(-1.0f, 0.0f, -0.01f), new Vector3(1.0f, 0.0f, -0.01f) };
    private Vector3 inputDir;
    private float maintainTime;
    private float elapsedTime;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        inputDir = rocalDir[Random.Range(0, rocalDir.Length)];
        if(Vector3.Distance(context.transform.position, context.targetPlayer.transform.position) > 9.0f)
        {
            maintainTime = 0.6f;
        }
        else
        {
            maintainTime = Random.Range(1.1f, 2.2f);
        }
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
        if ((_distance > 5.0f) && (elapsedTime < maintainTime))
        {
            context.FocusOnTarget(10f);
            context.Move_walk(inputDir, 4.5f+(0.7f*context.enemyPhase));
            return State.Running;
        }
        return State.Failure;
    }
}
