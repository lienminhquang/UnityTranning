using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereController : MonoBehaviour
{

    [HideInInspector] public bool boucing = true;
    public bool rollRequested = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void BounceStart()
    {
        boucing = true;
        //animator.SetBool("Bouncing", boucing);
        rollRequested = false;
    }

    void BounceFinished()
    {
        print(rollRequested);
        print(boucing);
        if(rollRequested == true && boucing == true)
        {
            boucing = false;
            rollRequested = false;
            animator.SetBool("Bouncing", boucing);
        }
    }

    internal void SetIdle()
    {
        animator.SetBool("Bouncing", true);
    }
}
