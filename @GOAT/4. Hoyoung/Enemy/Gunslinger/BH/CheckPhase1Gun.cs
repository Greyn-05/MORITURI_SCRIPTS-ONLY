using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckPhase1Gun : ActionNode
{
    protected override void OnStart() 
    {
        context.enemyControllerGun.modeling.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {

        if ((context.enemyPhase == 1) && (context._myState != BattleState.Guard_Parry))
        {
            if (context.health._hpModule.CurValue <= context.health._hpModule.MaxValue * 0.75)
            {
                context.enemyPhase = 2;
                context.SkillEnable = true;
                context.SkillCoolTime = 12.0f;
                Main.Player.OnResetLockOnClicked();
                context.enemyControllerGun.SetSmoke();
                return State.Failure;
            }
            return State.Success;
        }
        return State.Failure;
    }
}
