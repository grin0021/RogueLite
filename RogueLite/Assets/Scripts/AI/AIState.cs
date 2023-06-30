using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected Animator m_animator;

    public AIState(Animator animator)
    {
        m_animator = animator;
    }

    public abstract void OnStateEnter();
    public abstract void Update();
    public abstract void OnStateExit();
    public abstract string GetStateID();
}
