using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckStepEnable : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if(context.StepEnable == false)
        {
            return State.Failure;
        }
        return State.Success;
    }
}
