using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckEvadeDistance : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if (_distance < (3.5f + (context.enemyPhase-1)*0.7f) )
        {
            return State.Success;
        }
        return State.Failure;
    }
}
