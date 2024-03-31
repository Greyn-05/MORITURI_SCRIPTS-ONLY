using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckTuPhase3 : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (context.enemyPhase == 3)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
