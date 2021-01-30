using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody body;
    private SpriteRenderer sprite;
    private PlayerController player;

    private bool inView = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //HandleHit();
    }

    // In camera range?
    private void OnBecameInvisible()
    {
        inView = false;
    }

    // Ditto
    private void OnBecameVisible()
    {
        inView = true;
    }

    // Weak to bullets
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (player.vhsEnabled)
        {
            sprite.enabled = true;

            if (inView)
            {
                Vector3 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                body.rotation = Quaternion.Euler(0, angle, 0);
                direction.Normalize();
                body.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            }
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
