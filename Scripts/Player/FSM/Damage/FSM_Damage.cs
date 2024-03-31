using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FSM_Damage : FSM_Base
{
    private bool _isDamage;
    private bool _isForced;
    private float animNormalizedTime;
    
    public FSM_Damage(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        _isDamage = false;
        stateMachine.IsDamage = false;
        base.Enter();
        controller.Animator.SetTrigger("DamageTrigger");
        stateMachine.IsDamageAnimation = true;
        _isForced = false;
        stateMachine.IsForce = true;
    }

    public override void Exit()
    {
        base.Exit();
        controller.ForceReceiver.Reset();
        stateMachine.IsForce = false;
    }


    public override void Update()
    {
        
        ForceMove();
        
        if (IsAnimationPlaying("Damage", out animNormalizedTime))
        {
            _isDamage = true;
            TryForce();
        }
        else if (_isDamage && !IsAnimationPlaying("Damage", out animNormalizedTime))
        {
           
            stateMachine.IsDamageAnimation = false;
            stateMachine.ChangeState(stateMachine.GroundState);
        }
    }
   
    public override void PhysicsUpdate()
    {
        if (stateMachine.IsDamage)
        {
            stateMachine.ChangeState(stateMachine.DamageState);
        }
    }
    
    
    private void TryForce()
    {
        if (_isForced) return;
        _isForced = true;

        controller.ForceReceiver.Reset();
        controller.ForceReceiver.AddForce(stateMachine.KnockBackDirection);
    }
    
}
