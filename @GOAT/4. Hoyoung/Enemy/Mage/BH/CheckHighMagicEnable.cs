using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckHighMagicEnable : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {

    }

    protected override State OnUpdate() 
    {
        if (context.enemyControllerMage.HighMagicEnable)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
