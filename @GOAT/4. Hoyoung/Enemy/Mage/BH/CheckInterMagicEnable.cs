using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckInterMagicEnable : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.enemyControllerMage.InterMagicEnable)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
