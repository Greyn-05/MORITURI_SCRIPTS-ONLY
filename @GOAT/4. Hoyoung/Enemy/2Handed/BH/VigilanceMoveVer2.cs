using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class VigilanceMoveVer2 : ActionNode
{
    private float RestoreTime;
    private float elapsedTime;
    private Vector3 currentVector;
    private Vector3[] MoveVec = new Vector3[5] { new Vector3(1.0f, 0, 0.1f), new Vector3(1.0f, 0, 0.1f),  new Vector3(-1.0f, 0, 0.1f), new Vector3(-1.0f, 0, 0.1f), new Vector3(0.0f, 0, 1.0f) };
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        currentVector = MoveVec[Random.Range(0, MoveVec.Length)];
        RestoreTime = Random.Range(3.5f, 5.5f);
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
        elapsedTime += Time.deltaTime;
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if(_distance < 3.0f)
        {
            return State.Failure;
        }
        else if (elapsedTime < RestoreTime)
        {
            VigilanceMove();
            return State.Running;
        }
        return State.Failure;
    }

    private void VigilanceMove()
    {
        context.FocusOnTarget(7f);
        context.Move_walk(currentVector, 2.8f);
    }
}
