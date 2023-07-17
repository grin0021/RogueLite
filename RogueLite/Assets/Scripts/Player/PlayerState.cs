using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    Animator m_animator;

    public PlayerState(Animator animator)
    {
        m_animator = animator;
    }

    public abstract void OnEnter();

    // Update is called once per frame
    public abstract void Update();

    public abstract string GetStateID();

    public abstract void OnExit();
}
