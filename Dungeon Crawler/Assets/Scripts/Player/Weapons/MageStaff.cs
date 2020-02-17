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

    [Header("Components Assigment")]
    public GameObject lightingGameObject;
    public GameObject model;
    private Animator staffAnimator;
    private LightingBeam lightingBeam;

    // Start is called before the first frame update
    void Start()
    {
        lightingBeam = lightingGameObject.GetComponent<LightingBeam>();
        lightingBeam.damage = damage;
        lightingBeam.fireRate = fireRate;
        lightingGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(mouseButton))
        {
            lightingGameObject.SetActive(true);
            lightingBeam.isActive = true;
            // Begin Animation
        }

        if (Input.GetMouseButtonUp(mouseButton))
        {
            lightingBeam.isActive = false;
            lightingBeam.canDamage = false;
            // End Animation
            lightingGameObject.SetActive(false);
        }
    }
}
