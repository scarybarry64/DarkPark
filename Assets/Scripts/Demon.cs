﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    private SpriteRenderer falseForm, trueForm;

    private PlayerController player;
    private AudioManager audioManager;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
        falseForm = GetComponent<SpriteRenderer>();
        trueForm = transform.GetChild(0).GetComponent<SpriteRenderer>();
        falseForm.enabled = true;
        trueForm.enabled = false;
    }

    // Reveals true form
    public void Reveal()
    {
        falseForm.enabled = false;
        trueForm.enabled = true;
        audioManager.Play("Nazgul");

    }
}
