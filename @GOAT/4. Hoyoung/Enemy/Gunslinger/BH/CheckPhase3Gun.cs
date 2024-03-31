using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase3Gun : ActionNode
{
    protected override void OnStart() 
    {
        context.enemyControllerGun.modeling.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    protected override void OnStop() {
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
