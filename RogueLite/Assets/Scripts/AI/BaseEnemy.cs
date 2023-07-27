using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Tooltip("Movement speed of enemy character")]
    public float MoveSpeed;

    [Tooltip("Maximum distance enemy can detect player")]
    public float DetectRange;

    [Tooltip("Attack range of enemy")]
    public float AttackRange;

    [Tooltip("Enemy health")]
    public float MaxHealth;

    [Tooltip("Enemy sprite renderer")]
    public SpriteRenderer Sprite;

    protected Transform m_target;
    protected float m_currentHealth;
    protected bool bCanSeePlayer;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        m_currentHealth = MaxHealth;
        bCanSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
    }

    public abstract void MoveToTarget();
    public abstract void DetectPlayer();
    public abstract void Attack();
}
