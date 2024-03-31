using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SpawnAndProvoke : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {

    }

    protected override State OnUpdate() 
    {
        if(IsAnimationPlaying("Intro")==true)
        {
            return State.Running;
        }
        else
        {
            return State.Failure;
        }
    }
    bool IsAnimationPlaying(string animationName)
    {

        AnimatorStateInfo stateInfo = context.animationController.animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.IsName(animationName);
    }
}
