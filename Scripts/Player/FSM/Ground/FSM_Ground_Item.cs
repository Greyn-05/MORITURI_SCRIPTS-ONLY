using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM_Ground_Item : FSM_Ground
{


    #region Init
    public FSM_Ground_Item(FSM_Player stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (Main.Inven.IsQuickSlotEmpty())
        {
            stateMachine.ChangeState(stateMachine.GroundState);
            return;
        }

        WeelInputSubCancel();
        ItemAnimation();
        stateMachine.MovementSpeedModifier = 0;
        stateMachine.OnItemUseEvent += UseItem;
        //controller.OnDamageEvent += OnDamageState;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(animationData.ItemHash);
        stateMachine.OnItemUseEvent -= UseItem;
        WeelInputSubStart();
    }

    #endregion

    public override void Update()
    {
        if (stateMachine.MovementSpeedModifier > 0) stateMachine.MovementSpeedModifier = 0;
    }


    #region Method

    private void UseItem()
    {
        Main.Inven.QuickSlotItemUse();
        stateMachine.ChangeState(stateMachine.GroundState);
    }


    private void ItemAnimation()
    {
        Define.ItemType itemType = Main.Inven.inventory[Main.Inven.QuickSlotIndex].item.type;
        
        switch (itemType)
        {
            case Define.ItemType.consumables:
                controller.Animator.SetInteger(animationData.ItemIndexHash, 0);
                StartAnimation(animationData.ItemHash);
                return;
            case Define.ItemType.Production:
                controller.Animator.SetInteger(animationData.ItemIndexHash, 1);
                StartAnimation(animationData.ItemHash);
                return;
            default:
                controller.Animator.SetInteger(animationData.ItemIndexHash, -1);
                stateMachine.ChangeState(stateMachine.GroundState);
                return;
        }
    }

    private void WeelInputSubCancel()
    {
        controller.InputValue.PlayerActions.WheelUp.started -= controller.InputValue.DungeonWheelUpStarted;
        controller.InputValue.PlayerActions.WheelDown.started -= controller.InputValue.DungeonWheelDownStarted;
    }

    private void WeelInputSubStart()
    {
        controller.InputValue.PlayerActions.WheelUp.started += controller.InputValue.DungeonWheelUpStarted;
        controller.InputValue.PlayerActions.WheelDown.started += controller.InputValue.DungeonWheelDownStarted;
    }

    #endregion
    
    
}
