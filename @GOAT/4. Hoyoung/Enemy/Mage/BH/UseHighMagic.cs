using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class UseHighMagic : ActionNode
{
    private int curComboNum;
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        context.animationController.Skill = true;
        curComboNum = Random.Range(0, 2);
        //curComboNum = 0;
        context.animationController.ComboNum = curComboNum;
    }

    protected override void OnStop()
    {
        context._myState = BattleState.Stop;
        context.animationController.Skill = false;
        context.enemyControllerMage.StartHighCoolDown();
    }

    protected override State OnUpdate()
    {
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            return State.Running;
        }
        return State.Success;
    }
}
