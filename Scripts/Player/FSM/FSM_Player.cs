using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Serialization;

public class FSM_Player : FSM 
{ 
    #region Field
    public PlayerController PlayerController { get; }

    // States ----------------------------------------------------------------------------------------------------------
    public FSM_Ground GroundState { get; }
    public FSM_Ground_Dodge DodgeState { get; }
    public FSM_Ground_Item ItemState { get; }
    
    public FSM_Attack_Combo ComboState { get; }
    
    public FSM_Air_Fall FallState { get; }
   
    public FSM_Guard GuardState { get; }
    public FSM_Guard_ParryAttack ParryAttackState { get; }
    public FSM_Damage DamageState { get; }
    
    

    // StateMachine Data -----------------------------------------------------------------------------------------------
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; }
    public bool IsStoped { get; set; } = true;
    public bool IsMoved { get; set; }
    public bool IsRuned { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsGuard { get; set; }
    public bool IsGuardAnimation { get; set; }
    public bool IsLockOn { get; set; }
    public bool IsDamage { get; set; }
    public bool IsDamageAnimation { get; set; }
    public bool IsForce { get; set; }
    public int ComboIndex { get; set; } = -1;
    public bool IsParryBool { get; set; }
    
    public float JumpForce { get; set; }
    public Vector3 KnockBackDirection { get; set; }

    public Transform MainCameraTransform { get; set; }

    public Action OnItemUseEvent;
    #endregion


    #region Init
    public FSM_Player(PlayerController playerController)
    {
        this.PlayerController = playerController;
        GroundState = new FSM_Ground(this);
        DodgeState = new FSM_Ground_Dodge(this);
        ItemState = new FSM_Ground_Item(this);
        ComboState = new FSM_Attack_Combo(this);
        FallState = new FSM_Air_Fall(this);
        GuardState = new FSM_Guard(this);
        ParryAttackState = new FSM_Guard_ParryAttack(this);
        DamageState = new FSM_Damage(this);
        
        MainCameraTransform = Camera.main.transform;
        var GroundData = playerController.Data.GroundSO;

        MovementSpeed = Main.Player.Status.Speed.Value;
        RotationDamping = GroundData.BaseRotationDamping;
    }

    
    #endregion

    
}
