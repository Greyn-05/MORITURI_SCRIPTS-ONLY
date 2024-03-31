using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class DodgeLeft : ActionNode
{
    private float elapsedTime;
    protected override void OnStart() 
    {
        context.animationController.move_x = -1;
        context.animationController.move_z = 0;
        context.animationController.Play("Dodge", 0, 0);
        elapsedTime = 0f;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < context.DodgeTime)
        {
            context.FocusOnTarget(12f);
            Vector3 _rocalDirection = new Vector3(-1.0f, 0.0f, 0.0f);
            float speed = 5f;
            context.characterController.Move(context.transform.TransformDirection(_rocalDirection) * speed * Time.deltaTime);
            return State.Running;
        }
        return State.Success;
    }
}
