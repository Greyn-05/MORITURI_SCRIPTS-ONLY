using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ChooseComboAndAttackEasy : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.animationController.ComboNum = Random.Range(0, 4);
        //context.animationController.ComboNum = 3;
        context.animationController.AttackSpeed = 0.6f;
        context.animationController.Attack = true;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.AttackSpeed = 1.0f;
        context.animationController.Attack = false;
    }

    protected override State OnUpdate() 
    {
        /*AnimatorStateInfo currentInfo = context.animationController.animator.GetCurrentAnimatorStateInfo(0);
        if(currentInfo.normalizedTime <= 0.8f )
        {
            return State.Running;
        }*/
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if (_distance > 8.0f)
        {
            context.animationController.animator.SetTrigger("TooFarInCombo");
            context._myState = BattleState.Stop;
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(2f);
            return State.Running;
        }
        

        return State.Success;

    }
}
