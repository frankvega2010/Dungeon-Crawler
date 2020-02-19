using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public HealthBar healthBar;

    [Header("General Settings")]
    public GameObject player;
    public int maxHealth;

    [Header("Check Variables")]
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance >= 2.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1.5f * Time.deltaTime);
        }
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
