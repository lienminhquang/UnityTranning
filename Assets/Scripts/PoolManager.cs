﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject m_pickup;

    private Queue<GameObject> m_pickupsPool;
    private List<GameObject> m_activePickup;

    public static PoolManager instance = null;
    private bool enableSpawnPickup = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        m_activePickup = new List<GameObject>();
        m_pickupsPool = new Queue<GameObject>(12);
        for (int i = 0; i < 12; i++)
        {
            var free = Instantiate(m_pickup, transform);
            free.SetActive(false);
            m_pickupsPool.Enqueue(free);
        }
    }

    void Start()
    {
        Init();
    }

    GameObject Spawn()
    {
        if (m_pickupsPool.Count > 0)
        {
            var obj = m_pickupsPool.Dequeue();
            obj.SetActive(true);
            m_activePickup.Add(obj);
            var pickupw = obj.GetComponent<PickupWraper>();
            var pickup =  pickupw.m_pickUp;
            pickup.GetComponent<PickupController>().ReRandomType();
            pickup.GetComponent<BoxCollider>().enabled = true;
            return obj;
        }

        var free = Instantiate(m_pickup, transform);
        free.SetActive(true);
        free.GetComponent<PickupWraper>().m_pickUp.GetComponent<PickupController>().ReRandomType();
        free.GetComponent<PickupWraper>().m_pickUp.GetComponent<BoxCollider>().enabled = true;
        m_activePickup.Add(free);
        return free;

    }

    public GameObject GetTarget()
    {
        if(m_activePickup.Count > 0)
        {
            return m_activePickup[0];
        }

        return null;
    }
    public void DestroyPickup(GameObject obj, bool isDanger)
    {
        //obj.SetActive(false);
        
        if(isDanger)
        {
            obj.transform.parent.gameObject.GetComponent<PickupWraper>().m_pSFire.GetComponent<ParticleSystem>().Stop();
            obj.transform.parent.gameObject.GetComponent<PickupWraper>().m_explosionEffect.GetComponent<ParticleSystem>().Play();
        }

        obj.GetComponent<Animator>().SetTrigger("Death");
        var a = obj.GetComponent<BoxCollider>();
        a.enabled = false;
        m_activePickup.Remove(obj.transform.parent.gameObject);

    }

    public void CollectDeathPickup(GameObject obj)
    {
        
        m_pickupsPool.Enqueue(obj);
    }

    public void SetEnableSpawnPickup(bool isEnable)
    {
        enableSpawnPickup = isEnable;
    }

    // Update is called once per frame
    void Update()
    {
        if(enableSpawnPickup)
        {
            if (Input.GetButtonDown("Fire1") && UIController.GamePaused == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        Vector3 position = hit.point;
                        position.y = 0.5f;
                        GameObject obj = Spawn();
                        obj.transform.position = position;
                    }
                }
            }
        }
    }
}
