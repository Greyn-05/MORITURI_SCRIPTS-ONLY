using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class UseBasicMagic : ActionNode
{
    private int curComboNum;
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        context.animationController.Attack = true;
        curComboNum = Random.Range(0, 4);
        context.animationController.ComboNum = curComboNum;
    }

    protected override void OnStop()
    {
        context._myState = BattleState.Stop;
        context.animationController.Attack = false;
        context.enemyControllerMage.StartBasicCoolDown();
    }

    protected override State OnUpdate()
    {
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            if (curComboNum != 3)
            {
                context.FocusOnTarget(10f);
            }
            return State.Running;
        }
        return State.Success;
    }
}
