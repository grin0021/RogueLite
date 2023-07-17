using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AIState
{
    private List<Transform> m_patrolPoints;
    private Transform m_destination;

    public PatrolState(Animator animator)
        : base(animator)
    {
        m_patrolPoints = new List<Transform>();
    }

    public override void OnStateEnter()
    {
        var objects = GameObject.FindGameObjectsWithTag("Patrol Point");

        if (objects != null)
        {
            foreach(GameObject obj in objects)
            {
                m_patrolPoints.Add(obj.transform);
            }

            m_destination = m_patrolPoints[0];
        }
        else
        {
            Debug.LogError("Patrol points missing or mislabeled");
        }
    }

    public override void Update()
    {
        MoveToPoint();
    }

    public void MoveToPoint()
    { 

    }

    public override void OnStateExit()
    {
        
    }

    public override string GetStateID()
    {
        return "PatrolState";
    }
}