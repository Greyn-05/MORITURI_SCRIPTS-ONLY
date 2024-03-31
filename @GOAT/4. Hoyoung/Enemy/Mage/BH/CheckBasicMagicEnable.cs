using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckBasicMagicEnable : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if(context.enemyControllerMage.BasicMagicEnable)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
