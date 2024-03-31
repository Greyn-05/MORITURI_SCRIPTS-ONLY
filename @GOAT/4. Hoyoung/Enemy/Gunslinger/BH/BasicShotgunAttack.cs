using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BasicShotgunAttack : ActionNode
{
    private int[] attackIndex = new int[] { 0, 0, 0, 0, 1, 1 };
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        context.animationController.shotgunAttack = true;
        context.animationController.ComboNum = attackIndex[Random.Range(0, attackIndex.Length)];
        if((Random.Range(0,2)==0))
        {
            context.animationController.ComboNum = 2;
        }
        //context.animationController.ComboNum = 0;
    }

    protected override void OnStop()
    {
        context.animationController.shotgunAttack = false;
        context._myState = BattleState.Stop;
    }

    protected override State OnUpdate()
    {

        if (!context.enemyControllerGun.shotgunEnable)
        {
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }
        context.enemyControllerGun.StartShotgunAttackInterval();
        return State.Success;
    }
}
