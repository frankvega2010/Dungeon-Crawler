using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Settings")]
    public int maxHealth;

    [Header("Assign Components")]
    public MageStaff staffOfLighting;
    public HealthBar bar;

    [Header("Check Variables")]
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        bar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        health -= 0.1f;
        bar.SetHealth(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "laser")
        {

        }
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        bar.SetHealth(health);
        if(health <= 0)
        {
            health = 0;
            //Game Over.
        }
    }
}
