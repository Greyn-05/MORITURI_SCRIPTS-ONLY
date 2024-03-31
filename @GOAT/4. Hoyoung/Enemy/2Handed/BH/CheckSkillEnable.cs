using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckSkillEnable : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if(context.SkillEnable)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
