using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase2Gun : ActionNode
{
    protected override void OnStart() 
    {
        context.enemyControllerGun.modeling.transform.localEulerAngles = new Vector3(0, 0, 0);
    }


    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {

        if ((context.enemyPhase == 2) && (context._myState != BattleState.Guard_Parry))
        {
            if (context.health._hpModule.CurValue <= context.health._hpModule.MaxValue * 0.45)
            {
                context.enemyPhase = 3;
                context.SkillEnable = true;
                Main.Player.OnResetLockOnClicked();
                context.enemyControllerGun.SetSmoke();
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
