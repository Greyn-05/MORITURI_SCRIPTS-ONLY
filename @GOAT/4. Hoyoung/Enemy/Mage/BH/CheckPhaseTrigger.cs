using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhaseTrigger : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (context.enemyControllerMage.PhaseTrigger)
        {
            // Debug.Log("Change Phase");
            context.enemyControllerMage.StopPhaseTime();
            return State.Success;
        }
        return State.Failure;
    }
}
