using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBox : MonoBehaviour
{
    public delegate void OnBoxAction(string tag);
    public OnBoxAction OnBoxEnterCollider;
    public OnBoxAction OnBoxExitCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (OnBoxEnterCollider != null)
        {
            OnBoxEnterCollider(other.transform.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnBoxExitCollider != null)
        {
            OnBoxExitCollider(other.transform.tag);
        }
    }
}
