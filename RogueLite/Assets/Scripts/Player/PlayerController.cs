using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player movement speed")]
    public float PlayerSpeed = 10.0f;

    [Tooltip("Player jump power")]
    public float JumpPower = 1.0f;

    [Tooltip("Player sprite renderer")]
    public SpriteRenderer Sprite;

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

        transform.position += dir * PlayerSpeed * Time.deltaTime;
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

        transform.position += dir * PlayerSpeed * Time.deltaTime;
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
}
