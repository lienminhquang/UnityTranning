using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float m_moveSpeed = 0.02f;
    private Rigidbody m_rigidbody;
    public Text m_text;
    public Text m_winText;

    public GameObject m_pickupPool;
    private PoolManager m_poolManager;
    private Animator animator;
    public static int maxHeal = 50;
    public int m_heal = maxHeal;
    public int m_dangerDamage = 30;
    private GameObject m_pullingTarget = null;
    private BoxCollider m_boxCollider = null;

    public enum Stage
    {
        Unknown,
        Pulling,
        Dying,
        Falling,
        Landding
    }
    public Stage m_stage = Stage.Unknown;

    // Start is called before the first frame update
    void Start()
    {
        m_poolManager = m_pickupPool.GetComponent<PoolManager>();

        m_rigidbody = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider>();

        m_heal = maxHeal;
        m_text.text = "Heal: " + m_heal.ToString();
        m_winText.text = "";

        animator = GetComponent<Animator>();
        m_stage = Stage.Unknown;
        Respawn();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        switch (m_stage)
        {
            case Stage.Unknown:
                GameObject target = m_poolManager.GetTarget();
                if (target != null)
                {
                    Vector3 targetPos = target.transform.position;
                    targetPos.y = 0.0f;
                    transform.LookAt(targetPos, new Vector3(0f, 1f, 0f));
                    animator.SetFloat("Speed", 1);
                }
                else
                {
                    animator.SetFloat("Speed", 0);
                }
                break;
            case Stage.Pulling:
                break;
            case Stage.Falling:
                var pos = transform.position;
                if(pos.y > 0)
                {
                    Vector3 move = (pos - new Vector3(0f, 1f, 0f) * m_moveSpeed);
                    
                    m_rigidbody.MovePosition(move);
                }
                else
                {
                    transform.position = new Vector3(pos.x, 0f, pos.z);
                    animator.SetTrigger("Landing");
                    m_stage = Stage.Landding;
                }
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        switch (m_stage)
        {
            case Stage.Unknown:
                animator.SetFloat("Speed", 0);
                if (other.gameObject.CompareTag("Pickup"))
                {
                    if (other.gameObject.GetComponent<PickupController>().m_Type == PickupController.Type.Danger)
                    {
                        m_poolManager.DestroyPickup(other.gameObject, true);
                        animator.SetTrigger("Hit");
                        m_heal -= m_dangerDamage;
                       

                        if (m_heal <= 0)
                        {
                            m_stage = Stage.Dying;
                            m_boxCollider.enabled = false;
                            m_heal = 0;
                            animator.SetTrigger("Die");
                        }

                        m_text.text = "Heal: " + m_heal.ToString();
                        return;
                    }
                    else
                    {
                        animator.SetTrigger("Pulling");
                        m_pullingTarget = other.gameObject;
                        m_stage = Stage.Pulling;
                        m_boxCollider.enabled = false;
                    }
                }
                
                break;
            case Stage.Pulling:
                break;
            default:
                break;
        }
        
    }

    public void Respawn()
    {
        m_heal = maxHeal;
        m_text.text = "Heal: " + m_heal.ToString();

        m_boxCollider.enabled = false;
        transform.position = new Vector3(0f, 30f, 0f);
        animator.SetTrigger("Falling");
        m_stage = Stage.Falling;
    }

    public void PullingSuccess()
    {
        if(m_pullingTarget != null)
        {
            m_poolManager.DestroyPickup(m_pullingTarget, false);
            m_pullingTarget = null;
            m_boxCollider.enabled = true;
            m_stage = Stage.Unknown;
        }
    }

    public void LandingSuccess()
    {
        m_boxCollider.enabled = true;
        m_stage = Stage.Unknown;
        //transform.position = new Vector3(0f, 0f, 0f);
    }

    public void DyingFinished()
    {
        m_boxCollider.enabled = true;
        m_stage = Stage.Landding;
        Respawn();
    }

}
