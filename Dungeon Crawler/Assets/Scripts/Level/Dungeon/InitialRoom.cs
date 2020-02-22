using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get().initialRoom = this.gameObject;       
    }

   /* // Update is called once per frame
    void Update()
    {
        
    }*/
}
