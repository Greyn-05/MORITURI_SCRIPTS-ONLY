using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackTutorialP2 : ActionNode
{
    private int attackVar;
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        attackVar = Random.Range(0, 10);
        if (attackVar <5)
        {
            context.animationController.SAttack = true;
            //context.animationController.ComboNum = Random.Range(0, 3);
            context.animationController.ComboNum = 2;
        }
        else
        {
            context.animationController.Attack = true;
            context.animationController.ComboNum = Random.Range(0, 2);
        }
        context.animationController.AttackSpeed = 0.6f;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.AttackSpeed = 1.0f;
        if (attackVar < 5)
        {
            context.animationController.SAttack = false;
        }
        else
        {
            context.animationController.Attack = false;
        }
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if (_distance > 8.0f)
        {
            context.animationController.animator.SetTrigger("TooFarInCombo");
            context._myState = BattleState.Stop;
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(5f);
            return State.Running;
        }
        return State.Success;
    }
}
