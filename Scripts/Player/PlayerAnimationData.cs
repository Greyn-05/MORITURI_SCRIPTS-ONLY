using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationData 
{
    #region Field
    private string _groundParameterName = "@Ground";
    private string _idleParameterName = "Idle";
    private string _moveParameterName = "Move";
    private string _moveBattleParameterName = "MoveBattle";

    private string _airParameterName = "@Air";
    private string _jumpParameterName = "Jump";
    private string _fallParameterName = "Fall";

    private string _battleParameterName = "@Battle";
    
    private string _attackParameterName = "@Attack";
    private string _combo1ParameterName = "Combo";

    private string _guardParameterName = "Guard";
    private string _guardHitTriggerParameterName = "GuardHitTrigger";
    private string _parryAttackParameterName = "ParryAttack";
    private string _parryAttackTriggerParameterName = "ParryAttackTrigger";
    private string _parryAttackBoolParameterName = "ParryBool";
    private string _MovementModifier = "SpeedModifier";

    private string _itemParameterName = "Item";
    private string _itemIndexParameterName = "ItemIndex";

    private string _lockOn = "LockOn";
    private string _moveX = "Move_X";
    private string _moveY = "Move_Y";

    private string _die = "Die";
    
    public bool IsInit;
    #endregion


    #region Property
    public int GroundHash { get; private set; }
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int MoveBattleHash { get; private set; }

    public int AirHash { get; private set; }
    public int JumpHash { get; private set; }
    public int FallHash { get; private set; }


    public int BattleHash { get; private set; }
    
    public int AttackHash { get; private set; }
    public int Combo1Hash { get; private set; }
    
    public int GuardHash { get; private set; }
    public int GuardHitTriggerHash { get; private set; }
    public int ParryAttackHash { get; private set; }
    public int ParryAttackTriggerHash { get; private set; }
    public int ParryBoolHash { get; private set; }
    public int SpeedModifier { get; private set; }
    
    
    public int ItemHash { get; private set; }
    public int ItemIndexHash { get; private set; }
    
    public int LockOnHash { get; private set; }
    public int MoveXHash { get; private set; }
    public int MoveYHash { get; private set; }
    
    public int DieHash { get; private set; }
    
    #endregion


    #region Init
    public void InitializeAnimationData()
    {
        GroundHash = Animator.StringToHash(_groundParameterName);
        IdleHash = Animator.StringToHash(_idleParameterName);
        MoveHash = Animator.StringToHash(_moveParameterName);
        MoveBattleHash = Animator.StringToHash(_moveBattleParameterName);

        AirHash = Animator.StringToHash(_airParameterName);
        JumpHash = Animator.StringToHash(_jumpParameterName);
        FallHash = Animator.StringToHash(_fallParameterName);

        BattleHash = Animator.StringToHash(_battleParameterName);
        
        AttackHash = Animator.StringToHash(_attackParameterName);
        Combo1Hash = Animator.StringToHash(_combo1ParameterName);

        GuardHash = Animator.StringToHash(_guardParameterName);
        GuardHitTriggerHash = Animator.StringToHash(_guardHitTriggerParameterName);
        ParryAttackHash = Animator.StringToHash(_parryAttackParameterName);
        ParryAttackTriggerHash = Animator.StringToHash(_parryAttackTriggerParameterName);
        ParryBoolHash = Animator.StringToHash(_parryAttackBoolParameterName);
        
        SpeedModifier = Animator.StringToHash(_MovementModifier);

        ItemHash = Animator.StringToHash(_itemParameterName);
        ItemIndexHash = Animator.StringToHash(_itemIndexParameterName);
        
        LockOnHash = Animator.StringToHash(_lockOn);
        MoveXHash = Animator.StringToHash(_moveX);
        MoveYHash = Animator.StringToHash(_moveY);

        DieHash = Animator.StringToHash(_die);

        IsInit = true;
    }
    #endregion
}
