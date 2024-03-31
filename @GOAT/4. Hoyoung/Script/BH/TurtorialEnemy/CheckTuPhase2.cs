using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckTuPhase2 : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (context.enemyPhase == 2)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
