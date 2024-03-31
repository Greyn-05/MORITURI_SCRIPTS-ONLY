using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BasicPistolAttack : ActionNode
{
    private int[] attackIndex = new int[] { 0, 0, 0, 0, 1, 1, 2, 2 };
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.animationController.pistolAttack = true;
        context.animationController.ComboNum = attackIndex[Random.Range(0, attackIndex.Length)];
        //context.animationController.ComboNum = 1;
    }

    protected override void OnStop()
    {
        context.animationController.pistolAttack = false;
        context._myState = BattleState.Stop;

    }

    protected override State OnUpdate() 
    {

        if(!context.enemyControllerGun.pistolEnable)
        {
            return State.Failure;
        }
        if((context._myState == BattleState.Attack)|| (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }
        context.enemyControllerGun.StartPistolAttackInterval();
        return State.Success;
    }
}
