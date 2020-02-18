using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Settings")]
    public int maxHealth;

    [Header("Assign Components")]
    public MageStaff staffOfLighting;

    [Header("Check Variables")]
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "laser")
        {

        }
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0;
            //Game Over.
        }
    }
}
