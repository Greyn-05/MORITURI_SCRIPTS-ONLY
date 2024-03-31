using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Guard_ParryAttack : FSM_Guard
{
    #region Field
    private bool _isForced;
    private float animNormalizedTime;
    private AttackInfoData _attackInfoData;
    #endregion
    
    
    #region Init
    public FSM_Guard_ParryAttack(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Main.Player.ParryCount = 0;
        controller.OnStartIFrame(1f);
        Time.timeScale = 0.4f;
        StartAnimation(animationData.ParryAttackHash);
        controller.SetBattleState(BattleState.Attack);
        Main.Player.PlayerAttributeChange(Main.Player.PlayerAttribute);

        _isForced = false;
        
        _attackInfoData = controller.Data.AttakSO.GetAttackInfo(0);
        controller.Weapon.SetAttack(_attackInfoData.Damage);

        controller.OnParryAttackEvent += SetParryAttackCamera;
    }

    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1f;
        StopAnimation(animationData.ParryAttackHash);
        
        
        Main.Cinemachne.ResetParryAttackCamera();

        controller.OnParryAttackEvent -= SetParryAttackCamera;
    }

    public override void Update()
    {
        ForceMove();
        
        if (IsAnimationPlaying("ParryAttack", out animNormalizedTime))
        {
            TryForce();
            
            if (animNormalizedTime >= 0.8f)
            {
                stateMachine.IsGuard = false;
                stateMachine.ChangeState(stateMachine.GroundState);
            }
        }
        
    }
    #endregion
    

    #region Override
    protected override void OnGuardPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;   
    }

    protected override void OnGuardCanceled(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        if (animNormalizedTime < 0.8f) return;
        base.OnGuardCanceled(obj);
    }

    #endregion


    #region Method
    private void TryForce()
    {
        if (_isForced) return;
        _isForced = true;

        controller.ForceReceiver.Reset();
        var dir = -stateMachine.KnockBackDirection.normalized;
        controller.ForceReceiver.AddForce(dir * _attackInfoData.Force);
        
    }


    private void ParryMotion()
    {
        controller.Animator.SetTrigger(animationData.ParryAttackTriggerHash);
        stateMachine.IsParryBool = !stateMachine.IsParryBool;
        controller.Animator.SetBool(animationData.ParryBoolHash, stateMachine.IsParryBool);
    }

    private void SetParryAttackCamera(Transform target)
    {
        Main.Cinemachne.SetParryAttackCamera(target.transform);
    }

    #endregion
}
