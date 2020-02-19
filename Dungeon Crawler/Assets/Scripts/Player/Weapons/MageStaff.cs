using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageStaff : MonoBehaviour
{
    [Header("General Settings")]
    //public KeyCode attackButton;
    public int mouseButton;
    public float damage;
    public float fireRate;

    [Header("Charge Settings")]
    public float staffMaxCharge;
    public float staffChargeUsageMultipler;
    public float staffRechargeMultiplier;

    [Header("Checking Variables")]
    public bool canRecharge;
    public float staffCurrentCharge;

    [Header("Components Assigment")]
    public GameObject lightingGameObject;
    public GameObject model;
    private Animator staffAnimator;
    private LightingBeam lightingBeam;
    private bool doOnce;
    public StaffBar staffBar;

    // Start is called before the first frame update
    void Start()
    {
        lightingBeam = lightingGameObject.GetComponent<LightingBeam>();
        lightingBeam.damage = damage;
        lightingBeam.fireRate = fireRate;
        lightingGameObject.SetActive(false);
        staffCurrentCharge = staffMaxCharge;
    }

    // Update is called once per frame
    void Update()
    {
        if(staffCurrentCharge > 0)
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                canRecharge = false;
                lightingGameObject.SetActive(true);
                lightingBeam.isActive = true;
                // Begin Animation
            }

            if (Input.GetMouseButton(mouseButton))
            {
                //Debug.Log("in use");

                if(!canRecharge)
                {
                    staffCurrentCharge -= Time.deltaTime * staffChargeUsageMultipler;
                    staffBar.SetCharge(staffCurrentCharge);
                }

                if (staffCurrentCharge <= 0)
                {
                    if(!doOnce)
                    {
                        staffCurrentCharge = 0;
                        EndFiring();
                        doOnce = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(mouseButton))
        {
            EndFiring();
            doOnce = false;
        }

        if(canRecharge)
        {
            staffCurrentCharge += Time.deltaTime * staffRechargeMultiplier;
            staffBar.SetCharge(staffCurrentCharge);
            if(staffCurrentCharge >= staffMaxCharge)
            {
                staffCurrentCharge = staffMaxCharge;
                canRecharge = false;
            }
        }
    }

    public void EndFiring()
    {
        //Debug.Log("Recharging...");
        canRecharge = true;
        lightingBeam.ClearAll();
        lightingBeam.isActive = false;
        lightingBeam.canDamage = false;
        // End Animation
        lightingGameObject.SetActive(false);
    }
}
