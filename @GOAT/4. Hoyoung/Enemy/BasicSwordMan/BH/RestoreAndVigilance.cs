using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RestoreAndVigilance : ActionNode
{
    private float RestoreTime;
    private float elapsedTime;
    private Vector3 inputDir;
    private Vector3[] moveDir = new Vector3[] { new Vector3(1.0f, 0.0f, 0.1f), new Vector3(-1.0f, 0.0f, 0.1f), new Vector3(0.2f, 0.0f, 1.0f), new Vector3(-0.2f, 0.0f, 1.0f), };
    protected override void OnStart() 
    {
        RestoreTime = Random.Range(1.0f, 2.5f);
        elapsedTime = 0f;
        context._myState = BattleState.Move;
        inputDir = moveDir[Random.Range(0, moveDir.Length)];
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        elapsedTime += Time.deltaTime;

        if (_distance < 2.0f)
        {
            return State.Failure;
        }
        else if(elapsedTime < RestoreTime)
        {
            VigilanceMove();
            return State.Running;
        }
        return State.Success;
    }

    private void VigilanceMove()
    {
        context.FocusOnTarget(7f);
        context.Move_walk(inputDir, 2.0f);
    }
}
