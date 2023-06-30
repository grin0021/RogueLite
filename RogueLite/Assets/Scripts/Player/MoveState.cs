using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(Animator animator)
        : base(animator)
    {

    }

    public override void OnEnter()
    {
        
    }

    public override void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

        }
    }

    public override string GetStateID()
    {
        return "MoveState";
    }

    public override void OnExit()
    {
        
    }
}
