using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Ground_Dodge : FSM_Ground
{
    private float _dodgeX;
    private float _dodgeY;

    private bool _isDodge;
    private bool _isForced;
    private bool _isDodgeStarted;

    private Vector3 _dir;
    
    private float animNormalizedTime;

    //public event Action OnDodgeEvent;
    
    public FSM_Ground_Dodge(FSM_Player stateMachine) : base(stateMachine)
    {
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Enter()
    {
        _isDodge = false;
        _isForced = false;
        _isDodgeStarted = false;
        stateMachine.IsForce = true;
        base.Enter();
        UseStamina(10f);
        StartAnimation(animationData.GroundHash);
        _dodgeX = stateMachine.MovementInput.x;
        _dodgeY = stateMachine.MovementInput.y;

        if (_dodgeX == 0 && _dodgeY == 0) _dodgeY = 1;

        controller.InputValue.PlayerActions.Movement.started -= OnMovementStarted;
        controller.InputValue.PlayerActions.Movement.canceled -= OnMovementCanceled;
        Dodge();
    }

    public override void Exit()
    {
        
        base.Exit();
        StopAnimation(animationData.GroundHash);
        if(stateMachine.MovementInput == Vector2.zero) stateMachine.IsStoped = true;
        stateMachine.IsRuned = false;
        controller.ForceReceiver.Reset();
        stateMachine.IsForce = false;
    }


    
    public override void Update()
    {
        ForceMove();
        if (IsAnimationPlaying("Dodge", out animNormalizedTime))
        {
            _isDodge = true;
            TryForce();
            
        }


        if (_isDodge && !IsAnimationPlaying("Dodge", out animNormalizedTime))
        {
            if (stateMachine.IsAttacking) stateMachine.ChangeState(stateMachine.ComboState);

            if (_isDodgeStarted)
            {
                stateMachine.ChangeState(stateMachine.DodgeState);
                return;
            }
            stateMachine.ChangeState(stateMachine.GroundState);
        }
        
    }


    #region Method -----------------------------------------------------------------------------------------------------

    private void TryForce()
    {
        if (_isForced) return;
        _isForced = true;

        controller.ForceReceiver.Reset();

        var dir = _dir;
        dir.y = 0;
        controller.ForceReceiver.AddForce(dir * 10f);
        
    }
    
    private void Dodge()
    {
        controller.OnDodgeEvent?.Invoke();
        _dir = GetDodgeDirection();
        if (!stateMachine.IsLockOn) controller.transform.rotation = Quaternion.LookRotation(_dir);
        controller.Animator.SetFloat("Dodge_X", _dodgeX);
        controller.Animator.SetFloat("Dodge_Y", _dodgeY);
        controller.Animator.SetTrigger("DodgeTrigger");
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("Chanbara_Dodge"), controller.transform);
        
        controller.OnStartIFrame(0.5f);
    }
    
    private Vector3 GetDodgeDirection() // 방향얻기
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        if (stateMachine.MovementInput == Vector2.zero) return forward;
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }
    #endregion

    
    #region Override ---------------------------------------------------------------------------------------------------
    protected override void OnDodgeStarted(InputAction.CallbackContext ogj)
    {
        if (Time.timeScale < 1) return;
        if (controller.status.Stamina.CurValue < Define.Stamina_Dodge) return;
        if(animNormalizedTime > 0.8f && !_isDodgeStarted) _isDodgeStarted = true;
    }

    protected override void OnQuickSlotUseStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
    }

    #endregion
}
