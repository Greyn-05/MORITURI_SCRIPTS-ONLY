using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class MoveToCenter : ActionNode
{
    private Vector3 headDir;
    private float Ty;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
        headDir = context.floorCenter - context.transform.position;
        Ty = Quaternion.LookRotation(headDir).eulerAngles.y;
        headDir.Normalize();
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
        float _distance = Vector3.Distance(context.transform.position, context.floorCenter);
        AnimatorStateInfo currentInfo = context.animationController.animator.GetCurrentAnimatorStateInfo(0);
        if (!currentInfo.IsName("Ground"))
        {
            return State.Running;
        }
        if (_distance > 4.0f)
        {
            context.enemyControllerGun.modeling.transform.rotation = Quaternion.Lerp(context.enemyControllerGun.modeling.transform.rotation, Quaternion.Euler(new Vector3(0, Ty, 0)), Time.deltaTime * 10f);
            context.characterController.Move(headDir * (10.0f+2*(context.enemyPhase-1)) * Time.deltaTime);
            context.animationController.move_x = 0;
            context.animationController.move_z = 1;
            return State.Running;
        }
        return State.Success;
    }
}
