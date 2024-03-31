using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase1Mage : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if ((context.enemyPhase == 1))
        {
            if ((context.health._hpModule.CurValue <= context.health._hpModule.MaxValue * 0.80))
            {
                context.enemyControllerMage.PhaseTrigger = true;
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
