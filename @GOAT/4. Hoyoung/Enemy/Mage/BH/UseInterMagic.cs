using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class UseInterMagic : ActionNode
{
    private int curComboNum;
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.animationController.SAttack = true;
        curComboNum = Random.Range(0, 2);
        //curComboNum = 0;
        context.animationController.ComboNum = curComboNum;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.SAttack = false;
        context.enemyControllerMage.StartInterCoolDown();
    }

    protected override State OnUpdate() 
    {
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            if(curComboNum == 1)
            {
                context.FocusOnTarget(10f);
            }
            return State.Running;
        }
        return State.Success;
    }
}
