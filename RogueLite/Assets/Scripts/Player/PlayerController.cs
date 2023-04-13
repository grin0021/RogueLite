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

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void MoveLeft()
    {
        if (!Sprite.flipX)
        {
            Sprite.flipX = true;
        }

        Vector3 dir = new Vector3(-1.0f, 0.0f, 0.0f);

        transform.position += dir * PlayerSpeed * Time.deltaTime;
    }

    void MoveRight()
    {
        if (Sprite.flipX)
        {
            Sprite.flipX = false;
        }

        Vector3 dir = new Vector3(1.0f, 0.0f, 0.0f);

        transform.position += dir * PlayerSpeed * Time.deltaTime;
    }

    void Jump()
    {
        Vector2 force = new Vector2(0.0f, JumpPower);

        m_rigidBody.AddForce(force);
    }
}
