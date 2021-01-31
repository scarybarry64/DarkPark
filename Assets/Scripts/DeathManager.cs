using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public AudioManager audioManager;
    private bool deathFlag = false;

    private void Update()
    {
        if (!deathFlag)
        {
            deathFlag = true;
            audioManager.Play("Die");
        }
    }
}
