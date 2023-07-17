using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Tooltip("Character to follow")]
    public Transform Target;

    Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();

        if (!m_camera)
        {
            Debug.LogError("Camera component not found");
        }   
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (Target)
        {
            float camX = transform.position.x;
            float camY = transform.position.y;

            camX = Mathf.Lerp(camX, Target.position.x, 0.01f);
            camY = Mathf.Lerp(camY, Target.position.y, 0.01f);

            transform.position = new Vector3(camX, camY, transform.position.z);
        }
    }
}