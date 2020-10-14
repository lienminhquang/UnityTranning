using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject m_pickup;

    private Queue<GameObject> m_pickupsPool;
    private List<GameObject> m_activePickup;

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
            return obj;
        }

        var free = Instantiate(m_pickup, transform);
        free.SetActive(true);
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
    public void DestroyPickup(GameObject obj)
    {
        obj.SetActive(false);
        if(m_activePickup.Remove(obj))
        {
            m_pickupsPool.Enqueue(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
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
