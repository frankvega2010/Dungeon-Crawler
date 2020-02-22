using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCore : MonoBehaviour
{
    public delegate void OnObjectAction();
    public OnObjectAction OnObjectPickedUp;

    public string itemName;
    public int id;
    public GameObject icon;
    public bool dontDestroyOnLoad;
    public bool canBePickedUp = true;
    public bool isLastItem = false;

    private void Awake()
    {
        if(dontDestroyOnLoad)
        {
            //Object.DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PickupObject()
    {
        if(OnObjectPickedUp != null)
        {
            OnObjectPickedUp();
        }
    }
}
