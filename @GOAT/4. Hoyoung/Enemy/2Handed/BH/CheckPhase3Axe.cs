using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase3Axe : ActionNode
{
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if ((context.enemyPhase == 3) && (context._myState != BattleState.Guard_Parry))
        {
            return State.Success;
        }
        return State.Failure;
    }
}
