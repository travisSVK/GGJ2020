using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float m_cameraSpeed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // Move up.
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * Time.deltaTime * m_cameraSpeed;
        }
        
        // Move down.
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.up * Time.deltaTime * m_cameraSpeed;
        }

        // Move right.
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * m_cameraSpeed;
        }

        // Move left.
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * Time.deltaTime * m_cameraSpeed;
        }
    }
}
