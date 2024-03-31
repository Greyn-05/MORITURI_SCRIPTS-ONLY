using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class FSM_Base : IState
{
    #region Field ------------------------------------------------------------------------------------------------------
    protected FSM_Player stateMachine;
    protected readonly PlayerGroundSO groundData;
    protected readonly PlayerAnimationData animationData;

    protected PlayerController controller;
    protected bool _isUIOpened;

    #endregion

    #region Init -------------------------------------------------------------------------------------------------------
    public FSM_Base(FSM_Player _stateMachine)
    {
        stateMachine = _stateMachine;
        controller = stateMachine.PlayerController;
        groundData = controller.Data.GroundSO;
        animationData = controller.AnimationData;
        Main.UI.OnUIPopupOpenEvent += OpenUIMove;
        Main.UI.OnUIPopupCloseEvent += CloseUIMove;
    }

    private void OpenUIMove() => _isUIOpened = true;

    private void CloseUIMove()
    {
        _isUIOpened = false;
        if(stateMachine.MovementInput != Vector2.zero) stateMachine.IsStoped = false;
    }

    #endregion

    #region IState -----------------------------------------------------------------------------------------------------
    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }
    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }
    public virtual void Update()
    {
        if (Time.timeScale < 1) return;
        RegenStamina(Define.StaminaRegen);
        

        if (_isUIOpened) return; 
        Move();
    }

    public virtual void PhysicsUpdate()
    {
        // if (stateMachine.IsDamage && !stateMachine.IsDamageAnimation)
        // {
        //     stateMachine.ChangeState(stateMachine.DamageState);
        //     return;
        // }
    }
    #endregion


    #region InputAction ------------------------------------------------------------------------------------------------
    private void AddInputActionsCallbacks()
    {
        PlayerInputValue inputValue = controller.InputValue;
        if(!SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town))
        {
            inputValue.PlayerActions.Movement.started += OnMovementStarted;
            inputValue.PlayerActions.Movement.canceled += OnMovementCanceled;
            inputValue.PlayerActions.Run.performed += OnRunStarted;
            inputValue.PlayerActions.Run.canceled += OnRunCanceled;

            inputValue.PlayerActions.AttackL.performed += OnAttackLPerformed;
            inputValue.PlayerActions.AttackL.canceled += OnAttackCanceled;
            inputValue.PlayerActions.AttackR.performed += OnAttackRPerformed;
            inputValue.PlayerActions.AttackR.canceled += OnAttackCanceled;

            inputValue.PlayerActions.Parrying.performed += OnGuardPerformed;
            inputValue.PlayerActions.Parrying.canceled += OnGuardCanceled;

            inputValue.PlayerActions.Dodge.started += OnDodgeStarted;
            
            inputValue.PlayerActions.QuickSlotUse.started += OnQuickSlotUseStarted;

            controller.OnDamageEvent -= OnDamageState;
            controller.OnDamageEvent += OnDamageState;
        }

        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town))
        {
            inputValue.TownActions.Movement.started += OnMovementStarted;
            inputValue.TownActions.Movement.canceled += OnMovementCanceled;
            inputValue.TownActions.Run.performed += OnRunStarted;
            inputValue.TownActions.Run.canceled += OnRunCanceled;

        }
    }


    private void RemoveInputActionsCallbacks()
    {
        // if (씬이 전투)
        PlayerInputValue inputValue = controller.InputValue;
        if(!SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town))
        {
            inputValue.PlayerActions.Movement.started -= OnMovementStarted;
            inputValue.PlayerActions.Movement.canceled -= OnMovementCanceled;
            inputValue.PlayerActions.Run.performed -= OnRunStarted;
            inputValue.PlayerActions.Run.canceled -= OnRunCanceled;

            inputValue.PlayerActions.AttackL.performed -= OnAttackLPerformed;
            inputValue.PlayerActions.AttackL.canceled -= OnAttackCanceled;
            inputValue.PlayerActions.AttackR.performed -= OnAttackRPerformed;
            inputValue.PlayerActions.AttackR.canceled -= OnAttackCanceled;

            inputValue.PlayerActions.Parrying.performed -= OnGuardPerformed;
            inputValue.PlayerActions.Parrying.canceled -= OnGuardCanceled;

            inputValue.PlayerActions.Dodge.started -= OnDodgeStarted;
            inputValue.PlayerActions.QuickSlotUse.started -= OnQuickSlotUseStarted;
            
            controller.OnDamageEvent -= OnDamageState;
        }
        //if(씬이 마을)
        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town))
        {
            inputValue.TownActions.Movement.started -= OnMovementStarted;
            inputValue.TownActions.Movement.canceled -= OnMovementCanceled;
            inputValue.TownActions.Run.performed -= OnRunStarted;
            inputValue.TownActions.Run.canceled -= OnRunCanceled;

        }

    }
    #endregion

   

    #region Callback ---------------------------------------------------------------------------------------------------

    protected virtual void OnMovementStarted(InputAction.CallbackContext obj)
    {
        stateMachine.IsStoped = false;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext obj)
    {
        stateMachine.IsStoped = true;
    }
    protected virtual void OnRunStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        stateMachine.IsRuned = true;
    }
    protected virtual void OnRunCanceled(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        stateMachine.IsRuned = false;
    }
    
    protected virtual void OnAttackLPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        if (controller.status.Stamina.CurValue == 0) return;
        stateMachine.IsAttacking = true;
        stateMachine.ComboIndex = 1;
    }

    protected virtual void OnAttackRPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        if (controller.status.Stamina.CurValue == 0) return;
        stateMachine.IsAttacking = true;
        stateMachine.ComboIndex = 6;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        stateMachine.IsAttacking = false;
    }
    protected virtual void OnGuardPerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 1) return;
        if(!stateMachine.IsGuard) stateMachine.IsGuard = true;
    }
    protected virtual void OnGuardCanceled(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        stateMachine.IsGuard = false;
    }

    protected virtual void OnDodgeStarted(InputAction.CallbackContext ogj)
    {
        if (Time.timeScale < 1) return;
    }

    protected virtual void OnQuickSlotUseStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        //Main.Inven.QuickSlotItemUse();
    }
    
    #endregion
    

    #region Movement ---------------------------------------------------------------------------------------------------
    private void ReadMovementInput() // 입력한 값을 읽어오기
    {
        stateMachine.MovementInput = controller.InputValue.PlayerActions.Movement.ReadValue<Vector2>();
    }

    private void Move() // 방향 얻어 움직이기 명령부
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    private void Rotate(Vector3 movementDirection) // 바라보는 방향 설정
    {
        if (movementDirection != Vector3.zero)
        {
            Transform playerTransform = controller.Transform_Player;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            
            playerTransform.rotation = 
                Quaternion.Slerp(playerTransform.rotation, targetRotation, 
                    stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private void Move(Vector3 movementDirection) // 방향 얻어 움직이기 실행부
    {
        float movementSpeed = GetMovemenetSpeed();
        controller.Controller.Move(
            ((movementDirection * movementSpeed)
            + controller.ForceReceiver.Movement)
            * Time.deltaTime
            );
    }
    
    protected void ForceMove()
    {
        controller.Controller.Move(controller.ForceReceiver.Movement * Time.deltaTime);
    }
    
    private Vector3 GetMovementDirection() // 방향얻기
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    protected float GetMovemenetSpeed() // 속도 얻기 (State별로 SpeedModifier 조정)
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }
    #endregion






    #region Animation --------------------------------------------------------------------------------------------------
    protected void StartAnimation(int animationHash)
    {
        controller.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        controller.Animator.SetBool(animationHash, false);
    }
    
    
    protected float GetNormalizedTime(Animator animator,string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
    
    protected bool IsAnimationPlaying(string animationName, out float animNormalizedTime)
    {
        AnimatorStateInfo stateInfo = controller.Animator.GetCurrentAnimatorStateInfo(0);
        animNormalizedTime = stateInfo.normalizedTime;
        return (stateInfo.IsTag(animationName) && animNormalizedTime <= 1f);
    }
    #endregion


    #region Stamina

    protected void RegenStamina(float value)
    {
        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town)) return;
        controller.status.Stamina.AddValue(value * Time.deltaTime);
    }

    protected void UseStamina(float value)
    {
        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town)) return;
        controller.status.Stamina.SubValue(value);
    }
    

    #endregion


    #region Event

    protected void OnDamageState()
    {
        stateMachine.ChangeState(stateMachine.DamageState);
    }
    

    #endregion
}
