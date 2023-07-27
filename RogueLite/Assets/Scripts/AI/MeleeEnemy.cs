using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
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

    public override void MoveToTarget()
    {
        if (m_target)
        {
            Vector3 dir = m_target.transform.position - transform.position;

            dir.y = 0.0f;
            dir.z = 0.0f;

            if (dir.x > 0.0f)
            {
                Sprite.flipX = false;
            }
            else
            {
                Sprite.flipX = true;
            }

            if (Mathf.Abs(dir.x) > AttackRange)
            {
                dir.Normalize();
                transform.position += dir * MoveSpeed * Time.deltaTime;
            }
        }
    }

    public override void Attack()
    {

    }

    public override void DetectPlayer()
    {

    }
}
