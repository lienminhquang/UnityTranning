﻿using System.Collections;
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

    public GameObject m_cloth;
    public GameObject m_body;
    public Material m_normalClothMat;
    public Material m_normalBodyMat;
    public Material m_dissolveMat;

    public enum Stage
    {
        Unknown,
        Pulling,
        Dying,
        Falling,
        DyingEffect,
        Landding,
        //Free,
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
            case Stage.DyingEffect:
                {
                    if(m_body.GetComponent<SpawnEffect>().isPlaying == false && m_cloth.GetComponent<SpawnEffect>().isPlaying == false)
                    {
                        m_body.GetComponent<SkinnedMeshRenderer>().material = m_normalBodyMat;
                        m_cloth.GetComponent<SkinnedMeshRenderer>().material = m_normalClothMat;
                        
                        Respawn();
                    }
                    break;
                }
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
                            Die();
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

    private void Die()
    {
        m_stage = Stage.Dying;
        m_boxCollider.enabled = false;
        m_heal = 0;
        animator.SetTrigger("Die");
        PoolManager.instance.SetEnableSpawnPickup(false);
    }

    public void Respawn()
    {
        m_heal = maxHeal;
        m_text.text = "Heal: " + m_heal.ToString();

        m_boxCollider.enabled = false;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(0f, 15f, 0f);
        animator.SetTrigger("Falling");
        m_stage = Stage.Falling;

        CutsceneManager.instantiate.PlayPlaerRespawnCutscene();
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
        
        m_body.GetComponent<SkinnedMeshRenderer>().material = m_dissolveMat;
        m_body.GetComponent<SpawnEffect>().StartEffect(); 
        m_cloth.GetComponent<SkinnedMeshRenderer>().material = m_dissolveMat;
        m_cloth.GetComponent<SpawnEffect>().StartEffect();
        m_stage = Stage.DyingEffect;
        //Respawn();
    }

}
