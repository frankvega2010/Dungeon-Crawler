using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public Door houseDoor;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get().houseDoor = houseDoor;
    }
}
