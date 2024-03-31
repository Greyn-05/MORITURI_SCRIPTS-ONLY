using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase1Axe : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if((context.enemyPhase == 1)&&(context._myState != BattleState.Guard_Parry))
        {
            if(context.health._hpModule.CurValue <= context.health._hpModule.MaxValue*0.7)
            {
                context.enemyPhase = 2;
                context.SkillEnable = true;
                context.SkillCoolTime = 8.0f;
                context.StepCoolTime = 1.0f;
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
