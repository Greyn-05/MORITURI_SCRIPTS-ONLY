using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class FSM
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
