using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AfterDodgeAttack : ActionNode
{
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        context.animationController.Skill = true;
        context.animationController.SkillIndex = Random.Range(0, context.enemyPhase-1);
    }

    protected override void OnStop()
    {
        context._myState = BattleState.Stop;
        context.animationController.Skill = false;
    }

    protected override State OnUpdate()
    {

        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }

        return State.Success;
    }
}
