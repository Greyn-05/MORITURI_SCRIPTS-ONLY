using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RoundMoveNotFocus : ActionNode
{
    private Vector3[] rocalDir = new Vector3[] { new Vector3(-1.0f, 0.0f, -0.01f), new Vector3(1.0f, 0.0f, -0.01f) };
    private Vector3 inputDir;
    private float maintainTime;
    private float elapsedTime;
    private float Ty;
    protected override void OnStart()
    {
        context._myState = BattleState.Move;
        inputDir = rocalDir[Random.Range(0, rocalDir.Length)];
        if (Vector3.Distance(context.transform.position, context.targetPlayer.transform.position) > 9.0f)
        {
            maintainTime = 0.6f;
        }
        else
        {
            maintainTime = Random.Range(1.1f, 2.2f);
        }
        elapsedTime = 0f;
        Ty = Quaternion.LookRotation(context.transform.TransformDirection(inputDir)).eulerAngles.y;
    }

    protected override void OnStop()
    {
        context._myState = BattleState.Stop;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
        context.enemyControllerGun.modeling.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    protected override State OnUpdate()
    {
        if(!context.CheckRunningAnimation("Ground"))
        {
            return State.Running;
        }
        elapsedTime += Time.deltaTime;
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if ((_distance > 5.0f) && (elapsedTime < maintainTime))
        {
            context.enemyControllerGun.modeling.transform.rotation = Quaternion.Lerp(context.enemyControllerGun.modeling.transform.rotation, Quaternion.Euler(new Vector3(0, Ty, 0)), Time.deltaTime * 10f);
            context.characterController.Move(context.transform.TransformDirection(inputDir) * (7.0f + (0.5f * context.enemyPhase)) * Time.deltaTime);
            context.animationController.move_x = 0;
            context.animationController.move_z = 1;
            return State.Running;
        }
        return State.Failure;
    }
}
