using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBeam : MonoBehaviour
{
    [Header("General Settings")]
    public float fireRate;
    public float damage;

    [Header("Checking Variables")]
    public bool isActive;
    public bool canDamage;
    public List<EnemyController> enemiesTouching;
    public float fireRateTimer;
    // Start is called before the first frame update
    void Start()
    {
        canDamage = true;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            fireRateTimer += Time.deltaTime;

            if (fireRateTimer >= fireRate)
            {
                fireRateTimer = 0;
                canDamage = true;
            }

            if (enemiesTouching.Count > 0)
            {
                if(canDamage)
                {
                    for (int i = 0; i < enemiesTouching.Count; i++)
                    {
                        Debug.Log("Damage Given to enemy " + i + " : " + damage);
                        enemiesTouching[i].ReceiveDamage(damage);
                        //Make damage to all enemies that has touched.
                    }

                    canDamage = false;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            bool canEnterList = true;

            for (int i = 0; i < enemiesTouching.Count; i++)
            {
                if(enemiesTouching[i].gameObject == other.gameObject)
                {
                    canEnterList = false;
                }
            }

            if(canEnterList)
            {
                enemiesTouching.Add(other.gameObject.GetComponent<EnemyController>());
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
            enemiesTouching.Remove(other.gameObject.GetComponent<EnemyController>());
        }
    }

    public void ClearAll()
    {
        enemiesTouching.Clear();
    }
}
