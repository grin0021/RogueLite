using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player maximum health")]
    public float MaxHealth;

    [Tooltip("Player movement speed")]
    public float PlayerSpeed = 10.0f;

    [Tooltip("Player jump power")]
    public float JumpPower = 1.0f;

    [Tooltip("Player sprite renderer")]
    public SpriteRenderer Sprite;

    [Tooltip("Amount of damage player deals")]
    [SerializeField] protected float DamageFactor;

    [SerializeField] protected BoxCollider2D m_leftHitBox;
    [SerializeField] protected BoxCollider2D m_rightHitBox;

    float m_currentHealth;

    float m_speedMultiplier = 1.0f;

    float m_attackTimer = 0.0f;

    bool m_attack1 = false;
    bool m_attack2 = false;
    bool m_attack3 = false;

    Rigidbody2D m_rigidBody;
    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_animator.GetBool("Stun"))
        {
            if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }
            else if (m_animator.GetBool("bIsRunning"))
            {
                m_animator.SetBool("bIsRunning", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        HandleAttackTimer();
    }

    void EnableHitBox()
    {
        if (Sprite.flipX)
        {
            m_leftHitBox.enabled = true;
        }
        else
        {
            m_rightHitBox.enabled = true;
        }
    }

    void DisableHitBox()
    {
        if (m_leftHitBox.enabled)
        {
            m_leftHitBox.enabled = false;
        }

        if (m_rightHitBox.enabled)
        {
            m_rightHitBox.enabled = false;
        }
    }

    void MoveLeft()
    {
        if (!m_animator.GetBool("bIsRunning"))
        {
            m_animator.SetBool("bIsRunning", true);
        }

        if (!Sprite.flipX)
        {
            Sprite.flipX = true;
        }


        Vector3 dir = new Vector3(-1.0f, 0.0f, 0.0f);

        transform.position += dir * PlayerSpeed * m_speedMultiplier * Time.deltaTime;
    }

    void MoveRight()
    {
        if (!m_animator.GetBool("bIsRunning"))
        {
            m_animator.SetBool("bIsRunning", true);
        }

        if (Sprite.flipX)
        {
            Sprite.flipX = false;
        }

        Vector3 dir = new Vector3(1.0f, 0.0f, 0.0f);

        transform.position += dir * PlayerSpeed * m_speedMultiplier * Time.deltaTime;
    }

    void Jump()
    {
        if ((!m_attack1 || !m_attack2 || !m_attack3) && m_attackTimer <= 0.25f)
        {
            ResetAttack();

            if (m_animator.GetBool("bIsRunning"))
            {
                m_animator.SetBool("bIsRunning", false);
            }

            if (!m_animator.GetBool("bIsJumping"))
            {
                m_animator.SetBool("bIsJumping", true);

                Vector2 force = new Vector2(0.0f, JumpPower);
                m_rigidBody.AddForce(force);
            }
        }
    }

    void Attack()
    {
        if (!m_animator.GetBool("bIsJumping"))
        {
            if (!m_attack1 && !m_attack2 && !m_attack3)
            {
                m_attack1 = true;
                m_animator.SetBool("Attack1", true);
                m_speedMultiplier = 0.25f;
            }
            else if (m_attack1 && !m_attack2 && m_attackTimer > 0.5f)
            {
                m_attack2 = true;
                m_attack1 = false;

                m_animator.SetBool("Attack2", true);

                m_attackTimer = 0.0f;
            }
            else if (m_attack2 && !m_attack3 && m_attackTimer > 0.5f)
            {
                m_attack3 = true;
                m_attack2 = false;

                m_animator.SetBool("Attack3", true);

                m_attackTimer = 0.0f;
            }
        }
    }

    void HandleAttackTimer()
    {
        if (m_attack1 || m_attack2 || m_attack3)
        {
            m_attackTimer += Time.deltaTime;
        }
    }

    public void ResetAttack()
    {
        m_attack1 = false;
        m_attack2 = false;
        m_attack3 = false;

        m_animator.SetBool("Attack1", false);
        m_animator.SetBool("Attack2", false);
        m_animator.SetBool("Attack3", false);

        m_attackTimer = 0.0f;
        m_speedMultiplier = 1.0f;

        DisableHitBox();
    }

    void TakeDamage(float damage, Vector2 dir)
    {
        m_currentHealth -= damage;

        m_rigidBody.AddForce(dir * 200.0f);

        m_animator.SetBool("Stun", true);

        ResetAttack();
        m_leftHitBox.enabled = false;
        m_rightHitBox.enabled = false;

        m_animator.SetBool("bIsJumping", false);
        m_animator.SetBool("bIsRunning", false);
    }

    public float GetDamageFactor()
    {
        return DamageFactor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            BaseEnemy enemy = collision.gameObject.GetComponentInParent<BaseEnemy>();

            if (enemy)
            {
                Transform enemyTransform = collision.gameObject.transform;

                Vector3 dir = transform.position - enemyTransform.position;
                dir.z = 0.0f;
                dir.y = 1.0f;
                dir.Normalize();

                TakeDamage(enemy.GetDamageFactor(), dir);
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyDetection"))
        {
            BaseEnemy enemy = collision.gameObject.GetComponentInParent<BaseEnemy>();

            if (enemy)
            {
                enemy.DetectPlayer(transform);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (m_animator.GetBool("bIsJumping"))
            {
                m_animator.SetBool("bIsJumping", false);
            }
            else if (m_animator.GetBool("Stun"))
            {
                m_animator.SetBool("Stun", false);
            }
        }
    }
}