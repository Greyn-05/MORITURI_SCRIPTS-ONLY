using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class UseSkill : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Attack;
        context.animationController.SkillIndex = Random.Range(0, context.enemyPhase);
        //context.animationController.ComboNum = 3;
        context.animationController.AttackSpeed = 0.75f + 0.2f * (context.enemyPhase - 1);
        context.animationController.Skill = true;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
        context.animationController.Skill = false;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);
        
        if ((context._myState == BattleState.Guard_Parry)|| (_distance > 5.5f))
        {
            context.enemyController.StartCoolDown();
            return State.Failure;
        }
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }

        context.enemyController.StartCoolDown();
        return State.Success;
    }
}
