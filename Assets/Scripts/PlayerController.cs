using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_moveSpeed;
    private Rigidbody m_rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_moveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 move = new  Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        m_rigidbody.AddForce(move * m_moveSpeed);
    }
}
