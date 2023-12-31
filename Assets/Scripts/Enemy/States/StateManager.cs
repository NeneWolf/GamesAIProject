using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> currentState;

    protected bool isTransitioningState = false;


    public void TransitionState(EState key)
    {
        if (isTransitioningState) return; // Avoid entering a new state while transitioning

        isTransitioningState = true;

        if (currentState != null)
            currentState.ExitState();

        currentState = states[key];
        currentState.EnterState();

        isTransitioningState = false;
    }
}
