using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player movement speed")]
    public float PlayerSpeed = 10.0f;

    [Tooltip("Player jump power")]
    public float JumpPower = 1.0f;

    [Tooltip("Player sprite renderer")]
    public SpriteRenderer Sprite;

    float m_speedMultiplier = 1.0f;

    float m_attackTimer = 0.0f;

    bool m_attack1 = false;
    bool m_attack2 = false;
    bool m_attack3 = false;
    bool m_attackEnd = false;

    Rigidbody2D m_rigidBody;
    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

        HandleAttackTimer();
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
        if (m_animator.GetBool("bIsRunning"))
        {
            m_animator.SetBool("bIsRunning", false);
        }

        if (!m_animator.GetBool("bIsJumping"))
        {
            m_animator.SetBool("bIsJumping", true);
        }

        Vector2 force = new Vector2(0.0f, JumpPower);

        m_rigidBody.AddForce(force);
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
            else if (m_attack1 && !m_attack2 && m_attackTimer > 0.75f)
            {
                m_attack2 = true;
                m_attack1 = false;

                m_animator.SetBool("Attack2", true);

                m_attackTimer = 0.0f;
            }
            else if (m_attack2 && !m_attack3 && m_attackTimer > 0.75f)
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

        if (m_attack3 && m_attackTimer >= 0.5f)
        {
            m_attackEnd = true;
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
        }
    }

    public void ResetAttack()
    {
        if (m_attack1 && !m_attack2)
        {
            m_attack1 = false;

            m_animator.SetBool("Attack1", false);
        }
        else if (m_attack2 && !m_attack3)
        {
            m_attack2 = false;

            m_animator.SetBool("Attack2", false);
            m_animator.SetBool("Attack1", false);
        }
        else if (m_attackEnd)
        {
            m_attack3 = false;

            m_animator.SetBool("Attack3", false);
            m_animator.SetBool("Attack2", false);
            m_animator.SetBool("Attack1", false);
        }

        m_speedMultiplier = 1.0f;
        m_attackTimer = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}