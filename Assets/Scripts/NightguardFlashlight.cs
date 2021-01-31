using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightguardFlashlight : MonoBehaviour
{
    public LayerMask mask;

    public bool playerDetected = false;

    private void Awake()
    {
        mask = LayerMask.GetMask("Default");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Physics.Linecast(transform.position, other.transform.position, mask))
        {
            playerDetected = true;
            Debug.Log("HERE");
        }
    }
}
