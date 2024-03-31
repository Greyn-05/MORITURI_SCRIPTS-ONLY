using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Ground : FSM_Base
{
    private float movementSpeed = 0f;
    private float moveX = 0f;
    private float moveY = 0f;
    
    #region Init -------------------------------------------------------------------------------------------------------
    public FSM_Ground(FSM_Player stateMachine) : base(stateMachine)
    {
        controller.Animator.SetFloat(animationData.SpeedModifier, 0);
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(animationData.GroundHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.GroundHash);
    }

    public override void Update()
    {
        if (stateMachine.IsGuard && !stateMachine.IsGuardAnimation)
        {
            stateMachine.ChangeState(stateMachine.GuardState);
            return;
        }
        base.Update();
        
        
        if (stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }

        if (_isUIOpened) stateMachine.IsStoped = true;
        if (stateMachine.IsLockOn) OnMove_LockOn();
        else OnMove();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!controller.Controller.isGrounded &&
            controller.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
    #endregion

    
    #region Override ---------------------------------------------------------------------------------------------------
    protected virtual void OnAttack()
    {
        if (Time.timeScale < 1) return;
        stateMachine.ChangeState(stateMachine.ComboState);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext ogj)
    {
        if (Time.timeScale < 1) return;
        if (controller.status.Stamina.CurValue < Define.Stamina_Dodge) return;
        stateMachine.ChangeState(stateMachine.DodgeState);
    }

    protected override void OnQuickSlotUseStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale < 1) return;
        base.OnQuickSlotUseStarted(obj);
        stateMachine.ChangeState(stateMachine.ItemState);
    }

    #endregion


    #region Move -------------------------------------------------------------------------------------------------------

    private bool IsStaminaZero()
    {
        if (controller.status.Stamina.CurValue < Define.Stamina_Run/10)
        {
            stateMachine.IsRuned = false;
            return true;
        }

        return false;
    }
    
    private void OnMove()
    {
        if (stateMachine.IsStoped)
        {
            SetSpeed(-Time.deltaTime, 0, groundData.RunSpeedModifier);
        }
        else
        {
            if (stateMachine.IsRuned && !IsStaminaZero())
            {
                UseStamina(Define.Stamina_Run * Time.deltaTime);
                SetSpeed(Time.deltaTime*2, 0, groundData.RunSpeedModifier);
            }
            else
            {
                if (movementSpeed > groundData.WalkSpeedModifier)
                {
                    SetSpeed(-Time.deltaTime*2, groundData.WalkSpeedModifier, groundData.RunSpeedModifier);
                }
                else
                {
                    SetSpeed(Time.deltaTime, 0, groundData.WalkSpeedModifier);
                }
            }
        }

        if (movementSpeed > 0) controller.SetBattleState(BattleState.Move);
        else controller.SetBattleState(BattleState.Stop);
    }
    private void SetSpeed(float value, float min, float max)
    {
        movementSpeed = Mathf.Clamp(movementSpeed += value, min, max);
            
        controller.Animator.SetFloat(animationData.SpeedModifier, movementSpeed);
        stateMachine.MovementSpeedModifier = movementSpeed;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    private void OnMove_LockOn()
    {
        SetSpeedLockOn(ref moveX, ref moveY, stateMachine.MovementInput);
        
        controller.Animator.SetFloat(animationData.MoveXHash, moveX);
        controller.Animator.SetFloat(animationData.MoveYHash, moveY);
        
        OnMove();
    }

    private void SetSpeedLockOn(ref float moveX, ref float moveY, Vector2 input)
    {
        // 입력값 x
        if (moveX < input.x)
        {
            moveX = Mathf.Clamp(moveX += Time.deltaTime*5, -1, input.x);
            
        }
        else
        {
            moveX = Mathf.Clamp(moveX -= Time.deltaTime*5, input.x, 1);
        }

        // 입력값 y
        if (moveY < input.y)
        {
            moveY = Mathf.Clamp(moveY += Time.deltaTime*5, -1, input.y);
        }
        else
        {
            moveY = Mathf.Clamp(moveY -= Time.deltaTime*5, input.y, 1);
        }
        
        
    }

    #endregion


}
