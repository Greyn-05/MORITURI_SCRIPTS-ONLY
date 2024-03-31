using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FarRangeAttack : ActionNode
{
    private int selection;
    protected override void OnStart()
    {
        context._myState = BattleState.Attack;
        selection = Random.Range(0, 3);
        switch (selection)
        {
            case 0:
                context.animationController.pistolAttack = true;
                context.animationController.ComboNum = Random.Range(2, 5);
                break;
            case 1:
                context.animationController.shotgunAttack = true;
                context.animationController.ComboNum = 2;
                break;
            case 2:
                context.animationController.rifleAttack = true;
                context.animationController.ComboNum = Random.Range(1, 3);
                break;
        }

    }

    protected override void OnStop() 
    {
        switch (selection)
        {
            case 0:
                context.animationController.pistolAttack = false;
                break;
            case 1:
                context.animationController.shotgunAttack = false;
                break;
            case 2:
                context.animationController.rifleAttack = false;
                break;
        }
        context._myState = BattleState.Stop;
    }

    protected override State OnUpdate() 
    {
        if ((context._myState == BattleState.Attack) || (context._myState == BattleState.Attack_Judge))
        {
            context.FocusOnTarget(10f);
            return State.Running;
        }
        return State.Success;
    }
}
