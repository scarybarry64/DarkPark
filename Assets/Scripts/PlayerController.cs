using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Editor variables
    [SerializeField] private float speed, sensitivity, bulletSpeed, bulletDelay;
    [SerializeField] private GameObject bullet, bulletSpawn;
    [SerializeField] private AudioSource gunSound, vhsSound;

    [SerializeField] private AudioManager audioManager;

    // FLASHLIGHT VARIABLES
    [SerializeField] private Light flashlight;
    [SerializeField] private MeshCollider flashlightCollider;
    private bool flashlightOn = true;
    public bool detected = false; // Can guard see player?

    // Hidden variables
    private CharacterController controller;
    private Transform camera;
    [HideInInspector] public bool vhsEnabled = false;
    private bool vhsEnabledFlag = false;
    private float rotationX = 0f;
    private float fireTime = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        camera = transform.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fireTime = Time.time;
    }

    private void Update()
    {
        HandleMovement();
        HandleLooking();
        HandleFlashlight();
        //HandleVHS();
        //HandleShooting();

    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag("Enemy"))
    //    {
    //        Debug.Log("DEAD");

    //        SceneManager.LoadScene("MenuScene");
    //    }


    //    if (collider.CompareTag("Nightguard Flashlight")) // Player enters guard light
    //    {
    //        detected = true;

    //        Debug.Log("DETECTED!");
    //    }
    //}

    //private void OnTriggerExit(Collider collider) // Player leaves guard light
    //{
    //    if (collider.CompareTag("Nightguard Flashlight"))
    //    {
    //        detected = false;

    //        Debug.Log("NOT DETECTED!");
    //    }
    //}

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical"); // Unity uses xzy axis order
        Vector3 movement = transform.right * x + transform.forward * z;
        controller.Move(movement * speed * Time.deltaTime);
    }

    private void HandleLooking()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        camera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleFlashlight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!flashlightOn) // Turn on
            {
                flashlight.enabled = true;
                flashlightCollider.enabled = true;
                flashlightOn = true;
            }
            else // Turn off
            {
                flashlight.enabled = false;
                flashlightCollider.enabled = false;
                flashlightOn = false;
            }
        }
    }

    //private void HandleInteraction()
    //{

    //}

    //private void HandleVHS()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        vhsSound.Play();

    //        if (vhsEnabled)
    //        {
    //            vhsEnabled = false;

    //            Debug.Log("VHS Disabled");
    //        }
    //        else
    //        {
    //            vhsEnabled = true;

    //            Debug.Log("VHS Enabled");
    //        }
    //    }
    //}

    //private void HandleShooting()
    //{
    //    if (Input.GetMouseButtonDown(0) && (Time.time - fireTime >= bulletDelay))
    //    {
    //        GameObject myBullet = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity);
    //        myBullet.GetComponent<Bullet>().Fire(bulletSpeed, camera.transform.forward);
    //        //gunSound.GetComponent<AudioClip>()
    //        gunSound.Play();
    //        fireTime = Time.time;
    //    }
    //}


    
}