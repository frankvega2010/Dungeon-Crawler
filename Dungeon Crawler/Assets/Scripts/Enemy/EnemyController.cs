using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public HealthBar healthBar;

    [Header("General Settings")]
    public int maxHealth;

    [Header("Check Variables")]
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            health = 0;
            Debug.Log("dead");
            Destroy(gameObject);
        }
    }
}
