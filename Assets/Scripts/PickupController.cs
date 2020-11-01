using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{

    public enum Type
    {
        Normal,
        Danger
    }

    public Type m_Type = Type.Normal;

    // Start is called before the first frame update
    void Start()
    {
        ReRandomType();
    }

    public void ReRandomType()
    {
        int color = Random.Range(0, 3);

        m_Type = color == 1 ? Type.Danger : Type.Normal;
        print(color);
        GetComponent<Animator>().SetInteger("Color", color);
        if (m_Type == Type.Danger)
        {
            gameObject.transform.parent.gameObject.GetComponent<PickupWraper>().m_pSFire.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            gameObject.transform.parent.gameObject.GetComponent<PickupWraper>().m_pSFire.GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
