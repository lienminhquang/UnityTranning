using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float m_moveSpeed;
    private Rigidbody m_rigidbody;
    private int m_score;
    public Text m_text;
    public Text m_winText;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_moveSpeed = 10.0f;

        m_score = 0;
        SetScoreText();

        m_winText.text = "";
    }

    void SetScoreText()
    {
        m_text.text = "Score: " + m_score.ToString();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            m_score++;
            SetScoreText();

            if(m_score >= 5)
            {
                m_winText.text = "You Win!";
            }
        }
    }
}
