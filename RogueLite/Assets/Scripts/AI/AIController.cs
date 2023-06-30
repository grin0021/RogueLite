using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Tooltip("Enemy move speed")]
    public float MoveSpeed = 10.0f;

    [Tooltip("Enemy damage factor")]
    public float Damage = 10.0f;

    Animator m_animator;

    AIState m_currentState;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        if (m_animator)
        {
            SetState(new PatrolState(m_animator));
        }
        else
        {
            Debug.LogError("Issue with AI animator");
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.Update();
    }

    void SetState(AIState state)
    {
        if (m_currentState != null)
        {
            m_currentState.OnStateExit();
        }

        m_currentState = state;
        m_currentState.OnStateEnter();
    }
}