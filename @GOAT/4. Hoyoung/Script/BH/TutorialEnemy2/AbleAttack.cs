using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AbleAttack : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if((context._myState==BattleState.Attack)||(context._myState == BattleState.Attack_Judge) || (context._myState == BattleState.Gethit) || (context._myState == BattleState.Down) || (context._myState == BattleState.Death))
        {
            return State.Failure;
        }
        return State.Success;
    }
}
