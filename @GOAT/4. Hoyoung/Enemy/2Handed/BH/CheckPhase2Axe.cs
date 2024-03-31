using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase2Axe : ActionNode
{
    protected override void OnStart() 
    {

    }

    protected override void OnStop() 
    {

    }

    protected override State OnUpdate() 
    {
        if ((context.enemyPhase == 2) && (context._myState != BattleState.Guard_Parry))
        {
            
            if (context.health._hpModule.CurValue <= context.health._hpModule.MaxValue * 0.3)
            {
                context.enemyPhase = 3;
                context.SkillEnable = true;
                context.SkillCoolTime = 7.5f;
                context.enemyController.StartTwoHandAxePhase3();
                context.enemyController.TurnOnAura();
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
