using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase2Mage : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if ((context.enemyPhase == 2))
        {
            if ((context.health._hpModule.CurValue <= context.health._hpModule.MaxValue * 0.45))
            {
                context.enemyControllerMage.PhaseTrigger = true;
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
