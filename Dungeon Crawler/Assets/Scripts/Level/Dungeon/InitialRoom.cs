using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialRoom : MonoBehaviour
{
    public Door initialDoor;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get().initialRoom = this.gameObject;
        GameManager.Get().initialDoor = this.initialDoor;
    }

   /* // Update is called once per frame
    void Update()
    {
        
    }*/
}
