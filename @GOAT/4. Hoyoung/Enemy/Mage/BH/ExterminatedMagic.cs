using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ExterminatedMagic : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.DamageReduce = 90;
        context.animationController.Skill = true;
        context.animationController.ComboNum = 7;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.Skill = false;
        context.animationController.ComboNum = 0;
        context.enemyControllerMage.PhaseTrigger = false;
        context.enemyControllerMage.StartPhaseTime();
        if(context.enemyPhase < 3)
        {
            context.enemyPhase += 1;
            // Debug.Log("Start " + context.enemyPhase + "Phase");
        }
        context.DamageReduce = 0;
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
