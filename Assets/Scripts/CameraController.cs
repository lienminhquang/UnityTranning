using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_player;
    public Vector3 m_offset;

    // Start is called before the first frame update
    void Start()
    {
        m_offset = transform.position - m_player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void LateUpdate()
    {
        transform.position = m_player.transform.position + m_offset;
        transform.LookAt(m_player.transform, new Vector3(0.0f, 1.0f, 0.0f));
    }
}
