using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Guard : FSM_Base
{
    private string[] guardSound = 
        new string[] { 
            "SEKIRO_GUARD_1", "SEKIRO_GUARD_2", "SEKIRO_GUARD_3", 
            "SEKIRO_GUARD_4", "SEKIRO_GUARD_5", "SEKIRO_GUARD_6"
            
        };

    private string[] parrySound =
        new string[]
        {
            "SEKIRO_PARRY_1", "SEKIRO_PARRY_2", "SEKIRO_PARRY_3",
            "SEKIRO_PARRY_4", "SEKIRO_PARRY_5", "SEKIRO_PARRY_6"
        };

    private AudioClip[] _guardClip;

    private AudioClip[] _parryClip;
    //Random.Range(0, guardSound.Length)]
    public FSM_Guard(FSM_Player _stateMachine) : base(_stateMachine)
    {
        _guardClip = new AudioClip[guardSound.Length];
        for (int i = 0; i < guardSound.Length; i++)
        {
            _guardClip[i] = Main.Resource.Load<AudioClip>(guardSound[i]);
        }

        _parryClip = new AudioClip[parrySound.Length];
        for (int i = 0; i < parrySound.Length; i++)
        {
            _parryClip[i] = Main.Resource.Load<AudioClip>(parrySound[i]);
        }
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(animationData.GuardHash);
        if (!stateMachine.IsGuardAnimation)
        {
            controller.Animator.SetTrigger("GuardTrigger");
            //controller.Animator.Play("Revenge_Guard_Start_big", 0, 0);
            stateMachine.IsGuardAnimation = true;
        }

        controller.OnParryingEvent += IsParried;
        controller.OnGuardEvent += IsGuard;
        controller.SetBattleState(BattleState.Guard);
    }

    public override void Exit()
    {
        controller.OnParryingEvent -= IsParried;
        controller.OnGuardEvent -= IsGuard;
        stateMachine.IsGuardAnimation = false;
        stateMachine.IsGuard = false;
        base.Exit();
        StopAnimation(animationData.GuardHash);
        controller.Animator.ResetTrigger(animationData.ParryAttackTriggerHash);
    }


    public override void Update()
    {
        RegenStamina(Define.StaminaRegen/4);
        ForceMove();
    }

    
    #region Override
    protected override void OnGuardCanceled(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        base.OnGuardCanceled(obj);
        stateMachine.ChangeState(stateMachine.GroundState);
    }

    protected override void OnAttackLPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
    }

    #endregion

    private void IsGuard()
    {
        TryForce(0.6f);
        Main.Audio.SfxPlay(_guardClip[Random.Range(0, _guardClip.Length)], controller.transform);
    }
    
    private void IsParried()
    {
        Main.Audio.SfxPlay(_parryClip[Random.Range(0, _parryClip.Length)], controller.transform);
        controller._myState = BattleState.Guard;
        Main.Player.OnParryCountEvent?.Invoke();
        if (Main.Player.ParryCount < 3)
        {
            TryForce(0.3f);
            ParryMotion();
            return;
        }
        stateMachine.ChangeState(stateMachine.ParryAttackState);
    }
    
    private void ParryMotion()
    {
        controller.Animator.SetTrigger(animationData.ParryAttackTriggerHash);
        stateMachine.IsParryBool = !stateMachine.IsParryBool;
        controller.Animator.SetBool(animationData.ParryBoolHash, stateMachine.IsParryBool);
    }

    private void TryForce(float percent)
    {
        controller.ForceReceiver.Reset();
        controller.ForceReceiver.AddForce(stateMachine.KnockBackDirection * percent);
        
    }
}
