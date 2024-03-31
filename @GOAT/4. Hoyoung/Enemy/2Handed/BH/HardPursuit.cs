using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class HardPursuit : ActionNode
{
    private bool skillTrigger;
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;

        skillTrigger = true;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.AttackSpeed = 0.75f + 0.2f * (context.enemyPhase - 1);
        context.animationController.Skill = false;
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        if ((_distance > 2.5f) && (skillTrigger))
        {
            context.FocusOnTarget(20f);
            context.Move(new Vector3(0.0f, 0.0f, 1.0f), 20f);
            return State.Running;
        }
        if((_distance <= 2.5f)&&(skillTrigger))
        {
            context.animationController.Skill = true;
            context.animationController.SkillIndex =1;
            context.animationController.AttackSpeed = 1.5f;
            context._myState = BattleState.Attack;
            skillTrigger = false;
            return State.Running;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(5f);
            return State.Running;
        }
        return State.Success;
    }
}
