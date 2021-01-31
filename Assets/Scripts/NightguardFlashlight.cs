using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightguardFlashlight : MonoBehaviour
{
    public LayerMask layerMask;

    public bool playerDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Physics.Linecast(transform.position, other.transform.position, layerMask))
        {
            playerDetected = true;
            //Debug.Log("HERE");
        }
    }
}
