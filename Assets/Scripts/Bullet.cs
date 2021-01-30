using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector3 direction;

    // Spawns bullet with given speed and direction, then despawns after 3s
    public void Fire(float speed = 0, Vector3 direction = default)
    {
        this.speed = speed;
        this.direction = direction;
        this.transform.rotation = Quaternion.LookRotation(direction);
        Destroy(gameObject, 3f);
    }

    // Dissipate upon enemy impact
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    // Shoots bullet at speed towards direction
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
