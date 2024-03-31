using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Air_Fall : FSM_Air
{
    public FSM_Air_Fall(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(animationData.FallHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.FallHash);
    }

    public override void Update()
    {
        base.Update();

        if (controller.Controller.isGrounded)
        {
            stateMachine.ChangeState(stateMachine.GroundState);
            return;
        }
    }
}
