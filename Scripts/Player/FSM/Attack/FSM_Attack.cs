using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attack : FSM_Base
{
    public FSM_Attack(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(animationData.AttackHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.AttackHash);
    }

    
}
