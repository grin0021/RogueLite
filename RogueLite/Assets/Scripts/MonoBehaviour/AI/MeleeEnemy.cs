using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

/// <summary>
/// The basic script class for a melee enemy
/// </summary>
public class MeleeEnemy : BaseEnemy
{
    [Header("Attack hit boxes")]
    [SerializeField] protected BoxCollider2D m_leftHitBox;
    [SerializeField] protected BoxCollider2D m_rightHitBox;

    List<Transform> PatrolPoints;

    public GameObject PatrolPoint;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        GameObject point1 = Instantiate(PatrolPoint);
        Vector3 pos1 = transform.position;
        pos1.x -= 2.5f;
        
        point1.transform.position = pos1;

        GameObject point2 = Instantiate(PatrolPoint);
        Vector3 pos2 = transform.position;
        pos2.x += 2.5f;

        point2.transform.position = pos2;

        PatrolPoints = new List<Transform>();

        PatrolPoints.Add(point1.transform);
        PatrolPoints.Add(point2.transform);

        m_target = PatrolPoints[0];
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        if (!m_animator.GetBool("Stun"))
        {
            MoveToTarget();
        }
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

    void EndAttackState()
    {
        m_animator.SetBool("bIsAttacking", false);
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
            else if (Mathf.Abs(dir.x) <= AttackRange)
            {
                if (m_target == PatrolPoints[0])
                {
                    m_target = PatrolPoints[1];
                }
                else if (m_target == PatrolPoints[1])
                {
                    m_target = PatrolPoints[0];
                }
                else if (m_target.gameObject.name == "Player")
                {
                    Attack();
                }
            }
        }
    }

    public override void Attack()
    {
        m_animator.SetBool("bIsAttacking", true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

            if (player && !m_animator.GetBool("Stun"))
            {
                Transform playerTransform = collision.gameObject.transform;

                Vector3 dir = transform.position - playerTransform.position;
                dir.z = 0.0f;
                dir.y = 1.0f;
                dir.Normalize();

                TakeDamage(player.GetDamageFactor(), dir);
                m_animator.SetBool("Stun", true);
                m_animator.SetBool("bIsAttacking", false);
                DisableHitBox();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (m_animator.GetBool("Stun"))
            {
                m_animator.SetBool("Stun", false);
            }
        }
    }
}