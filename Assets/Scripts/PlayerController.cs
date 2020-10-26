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

    public GameObject m_pickupPool;
    private PoolManager m_poolManager;
    public PlayerSphereController playerSphereController;
    public float timeToTranslateToIdle = 0.5f;
    private float timeToTranslateToIdleCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_poolManager = m_pickupPool.GetComponent<PoolManager>();

        m_rigidbody = GetComponent<Rigidbody>();
        m_moveSpeed = 0.2f;

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
        GameObject target = m_poolManager.GetTarget();
        if(target != null)
        {
            if (playerSphereController.boucing == true)
            {
                playerSphereController.rollRequested = true;
            }
            else
            {
                Vector3 move = (target.transform.position - transform.position).normalized * m_moveSpeed;
                //transform.Translate(move);
                m_rigidbody.MovePosition(transform.position + move * m_moveSpeed);
                transform.LookAt(target.transform, new Vector3(0f, 1f, 0f));
            }
        }

    }

    private void LateUpdate()
    {
        if(m_rigidbody.velocity.sqrMagnitude <= float.Epsilon && playerSphereController.boucing == false)
        {
            timeToTranslateToIdleCounter += Time.deltaTime;
            if(timeToTranslateToIdleCounter >= timeToTranslateToIdle)
            {
                playerSphereController.SetIdle();
                timeToTranslateToIdleCounter = 0f;
            }
        }
        else
        {
            timeToTranslateToIdleCounter = 0f;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            m_poolManager.DestroyPickup(other.gameObject);

            m_score++;
            SetScoreText();

            //if(m_score >= 5)
            //{
            //    m_winText.text = "You Win!";
            //}
        }
    }
}
