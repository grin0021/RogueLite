using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// An abstract script class to provide all of the base requirements for any enemy class
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{
    [Tooltip("Movement speed of enemy character")]
    [SerializeField] protected float MoveSpeed;                 // Movement speed of enemy

    [Tooltip("Maximum distance enemy can detect player")]
    [SerializeField] protected float DetectRange;               // Range at which enemy can detect player

    [Tooltip("Attack range of enemy")]
    [SerializeField] protected float AttackRange;               // Range at which enemy can attack player

    [Tooltip("Attack rate of enemy")]
    [SerializeField] protected float AttackRate;                // Rate at which enemy can attack player

    [Tooltip("Damage dealt to player")]
    [SerializeField] protected float Damage;

    [Tooltip("Enemy health")]
    public float MaxHealth;                                     // Maximum health of the enemy

    protected Animator m_animator;                              // Animator component attached to game object
    protected SpriteRenderer m_sprite;                          // SpriteRenderer component attached to game object
    protected Transform m_target;                               // Target transfor for enemy to move towards
    protected Rigidbody2D m_rigidBody;
    protected float m_attackTimer;                              // Compliments attack timer to implement attack rate
    protected float m_currentHealth;                            // Current health value of the enemy
    protected bool bCanSeePlayer;                               // Can enemy see the player

    // Start is called before the first frame update
    virtual protected void Start()
    {
        // Initialize base values

        m_currentHealth = MaxHealth;
        bCanSeePlayer = false;
        m_attackTimer = AttackRate;

        m_sprite = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (m_currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, Vector2 dir)
    {
        m_currentHealth -= damage;
        m_rigidBody.AddForce(dir * 200.0f);
    }

    public float GetDamageFactor()
    {
        return Damage;
    }

    public void DetectPlayer(Transform player)
    {
        m_target = player;
    }

    public abstract void MoveToTarget();
    public abstract void Attack();
}