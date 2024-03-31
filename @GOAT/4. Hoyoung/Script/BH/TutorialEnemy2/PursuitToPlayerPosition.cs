using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PursuitToPlayerPosition : ActionNode
{
    protected override void OnStart() 
    {
        context._myState = BattleState.Move;
    }

    protected override void OnStop() 
    {
        context._myState = BattleState.Stop;
    }

    protected override State OnUpdate() 
    {
        float _distance = Vector3.Distance(context.transform.position, context.targetPlayer.transform.position);

        if (_distance > 1.5f)
        {
            ChasePlayer();
            return State.Running;
        }
        return State.Success;
    }

    private void ChasePlayer()
    {
        context.FocusOnTarget(12f);

        context.Move(new Vector3(0.0f, 0.0f, 1.0f), 7.5f);
        #region regacy
        /*Vector3 _rocalDirection = new Vector3(0.0f, 0.0f, 1.0f);
        float speed = 4.5f;
        context.characterController.Move(context.transform.TransformDirection(_rocalDirection) * speed * Time.deltaTime);
        context.animationController.move_x = _rocalDirection.x;
        context.animationController.move_z = _rocalDirection.z;
        
         Vector3 _targetPosition = new Vector3(context.targetPlayer.transform.position.x, context.transform.position.y, context.targetPlayer.transform.position.z);
        Vector3 _lookPosition = _targetPosition - context.transform.position;
        context.transform.rotation = Quaternion.Lerp(context.transform.rotation, Quaternion.LookRotation(_lookPosition), Time.deltaTime * 7f);
         
         */
        #endregion

    }
}
