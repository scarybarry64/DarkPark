using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarqueStatue : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerController player;

    //private bool stalking = false;
    private bool inCamera = false;
    private LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.GetMask("Default");
    }

    private void Update()
    {
        // Face player at all times
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        body.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();

        // Move towards player if not in camera view
        if (inCamera && !Physics.Linecast(transform.position, player.transform.position, mask))
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            //body.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            agent.SetDestination(player.transform.position);
        }
    }

    // Track visiblity
    private void OnBecameInvisible()
    {
        inCamera = false;
    }

    // If within camera view
    private void OnBecameVisible()
    {
        inCamera = true;
    }
}
