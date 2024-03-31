using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EvadeDodge : ActionNode
{
    private float elapsedTime;
    private Vector3[] stepAniVec = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector3(0, 0, -1) };
    private Vector3 currentVector;
    protected override void OnStart()
    {
        currentVector = stepAniVec[Random.Range(0, stepAniVec.Length)];
        context.animationController.move_x = currentVector.x;
        context.animationController.move_z = currentVector.z;
        context.animationController.Play("Dodge", 0, 0);
        elapsedTime = 0f;
    }

    protected override void OnStop()
    {
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;

    }

    protected override State OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < context.DodgeTime)
        {
            context.FocusOnTarget(5f);
            context.characterController.Move(context.transform.TransformDirection(currentVector) * 8.0f * Time.deltaTime);
            return State.Running;
        }
        return State.Success;
    }
}
