using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public delegate void OnPotionAction(float hp);
    public static OnPotionAction OnPotionTouchPlayer;

    public float healthRecover;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            if (OnPotionTouchPlayer != null)
            {
                OnPotionTouchPlayer(healthRecover);
            }

            Destroy(gameObject);
        }
        
    }
}
