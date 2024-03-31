using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Attack_Combo : FSM_Attack
{
    #region Field
    private bool isForced;
    private bool isComboed;
    private bool isEndIndex;

    private AttackInfoData _attackInfoData;
    #endregion

    #region Init
    public FSM_Attack_Combo(FSM_Player _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        controller.SetBattleState(BattleState.Attack);
        base.Enter();
        StartAnimation(animationData.Combo1Hash);
        UseStamina(Define.Stamina_Attack);
        isForced = false;
        isComboed = false;
        isEndIndex = false;
        
        int comboIndex = stateMachine.ComboIndex;
        _attackInfoData = controller.Data.AttakSO.GetAttackInfo(comboIndex);
        controller.Animator.SetInteger("ComboIndex", comboIndex);
        
        controller.Weapon.SetAttack((int)controller.status.Atk.Value + _attackInfoData.Damage);
        controller.OnAttackEventStarted();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.Combo1Hash);
        stateMachine.IsAttacking = false;
        if (!isComboed)
        {
            stateMachine.ComboIndex = -1;
        }
    }
    #endregion


    private void TryCombo()
    {
        if (isComboed) return;
        if (_attackInfoData.ComboStateIndex_L == -1 && _attackInfoData.ComboStateIndex_R == -1) return;
        if (!stateMachine.IsAttacking) return;
        
        isComboed = true;
    }

    private void TryForce()
    {
        if (isForced) return;
        isForced = true;

        controller.ForceReceiver.Reset();

        controller.ForceReceiver.AddForce(controller.transform.forward * _attackInfoData.Force);
    }

    public override void Update()
    {
        ForceMove();
        
        if (stateMachine.IsGuard && !stateMachine.IsGuardAnimation)
        {
            stateMachine.ChangeState(stateMachine.GuardState);
            return;
        }
        
        float normalizedTime = GetNormalizedTime(controller.Animator, "Attack");
        if (normalizedTime < 0.6f)
        {
            
            if (normalizedTime >= _attackInfoData.ForceTransitionTime)
            {
                TryForce();
            }

            if (normalizedTime >= _attackInfoData.ComboTransitionTime)
            {
                TryCombo();
            }
        }
        else
        {
            
            if (isComboed && !isEndIndex)
            {
                stateMachine.ChangeState(stateMachine.ComboState);
                controller.Weapon.SetAttack(_attackInfoData.Damage);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.GroundState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    protected override void OnAttackLPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        isEndIndex = false;
        if (controller.status.Stamina.CurValue == 0) return;
        stateMachine.IsAttacking = true;
        if (_attackInfoData.ComboStateIndex_L == -1) isEndIndex = true;
        stateMachine.ComboIndex = _attackInfoData.ComboStateIndex_L;
    }

    protected override void OnAttackRPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        isEndIndex = false;
        if (controller.status.Stamina.CurValue == 0) return;
        stateMachine.IsAttacking = true;
        if (_attackInfoData.ComboStateIndex_R == -1) isEndIndex = true;
        stateMachine.ComboIndex = _attackInfoData.ComboStateIndex_R;
    }
}
