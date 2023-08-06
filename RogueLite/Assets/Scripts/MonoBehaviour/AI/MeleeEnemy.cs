using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic script class for a melee enemy
/// </summary>
public class MeleeEnemy : BaseEnemy
{
    [Header("Attack hit boxes")]
    [SerializeField] protected BoxCollider2D m_leftHitBox;
    [SerializeField] protected BoxCollider2D m_rightHitBox;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        m_target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    void EnableHitBox()
    {
        if (m_sprite.flipX)
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
        if (m_sprite.flipX)
        {
            m_leftHitBox.enabled = false;
        }
        else
        {
            m_rightHitBox.enabled = false;
        }
    }

    public override void MoveToTarget()
    {
        if (m_target)
        {
            Vector3 dir = m_target.transform.position - transform.position;

            dir.y = 0.0f;
            dir.z = 0.0f;

            if (dir.x > 0.0f)
            {
                m_sprite.flipX = false;
            }
            else
            {
                m_sprite.flipX = true;
            }

            if (Mathf.Abs(dir.x) > AttackRange && !m_animator.GetBool("bIsAttacking"))
            {
                dir.Normalize();
                transform.position += dir * MoveSpeed * Time.deltaTime;

                m_animator.SetBool("bIsRunning", true);
            }
            else if (Mathf.Abs(dir.x) <= AttackRange && m_target.gameObject.name == "Player")
            {
                if (m_attackTimer >= AttackRate)
                {
                    Attack();
                }
            }
            else
            {

            }
        }
    }

    public override void Attack()
    {
        m_animator.SetBool("bIsAttacking", true);
    }

    public override void DetectPlayer()
    {

    }
}