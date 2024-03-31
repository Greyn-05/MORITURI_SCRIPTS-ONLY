using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BasicRifleAttack : ActionNode
{
    private int[] attackIndex = new int[] { 0, 0, 0, 0, 1, 1 };
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        context.animationController.rifleAttack = true;
        //context.animationController.ComboNum = attackIndex[Random.Range(0, attackIndex.Length)];
        if(context.SkillEnable==true)
        {
            context.animationController.ComboNum = 1;
        }
        else
        {
            context.animationController.ComboNum = 0;
        }
        
    }

    protected override void OnStop()
    {
        context.animationController.rifleAttack = false;
        context._myState = BattleState.Stop;

    }

    protected override State OnUpdate()
    {

        if (!context.enemyControllerGun.rifleEnable)
        {
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }

        if(context.animationController.ComboNum == 1)
        {
            context.startCoolDown();
        }
        else
        {
            context.enemyControllerGun.StartRifleAttackInterval();
        }
        
        return State.Success;
    }
}
