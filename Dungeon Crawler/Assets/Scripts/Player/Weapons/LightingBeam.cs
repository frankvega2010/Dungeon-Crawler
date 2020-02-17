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
    public bool isTouching;
    public List<GameObject> enemiesTouching;
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
                //canDamage = true;
            }

            /*if (canDamage)
            {
                canDamage = false;
            }*/
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            Debug.Log("Damage Given: " + damage);
            isTouching = true;
            enemiesTouching.Add(other.gameObject);

            /*if (canDamage)
            {
                
            }*/
        }
    }
}
