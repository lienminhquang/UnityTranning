using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        int color = Random.Range(0, 3);
        print(color);
        GetComponent<Animator>().SetInteger("Color",color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
