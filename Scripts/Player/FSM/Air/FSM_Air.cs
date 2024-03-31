using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Air : FSM_Base
{
    public FSM_Air(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(animationData.AirHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.AirHash);
    }
}
