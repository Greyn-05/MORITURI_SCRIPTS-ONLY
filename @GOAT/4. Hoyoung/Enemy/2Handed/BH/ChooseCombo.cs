using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ChooseCombo : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.animationController.ComboNum = Random.Range(0, 4);
        //context.animationController.ComboNum = 3;
        context.animationController.AttackSpeed = 0.75f + 0.3f*(context.enemyPhase-1);
        context.animationController.Attack = true;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.Attack = false;
    }

    protected override State OnUpdate() 
    {
        if ((context._myState == BattleState.Guard_Parry))
        {
            return State.Failure;
        }
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        AnimatorStateInfo currentInfo = context.animationController.animator.GetCurrentAnimatorStateInfo(0);
        if ((_distance > 12.0f)&&(!currentInfo.IsTag("aerial")))
        {
            context.animationController.animator.SetTrigger("TooFarInCombo");
            context._myState = BattleState.Stop;
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }
        
        return State.Success;
       
    }
}
