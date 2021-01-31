using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarqueStatue : MonoBehaviour
{

    public LayerMask layerMask;


    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private PlayerController player;

    //private bool stalking = false;
    private bool inCamera = false;
    private int anger = 0; // more quarters -> more anger -> faster movement
    //public int maxAnger = 6;
    private bool stunned = false; // player flashlight temp stuns
    private float timeSinceStunned;
    public float stunDuration; // how long stuns lasts

    private void Awake()
    {
        agent.speed = anger;
    }

    private void Update()
    {
        // Face player at all times
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        body.rotation = Quaternion.Euler(0, angle, 0);
        direction.Normalize();

        // Move towards player if not in camera view or stunned
        if ((inCamera && !Physics.Linecast(transform.position, player.transform.position, layerMask)) || stunned)
        {
            //agent.SetDestination(transform.position);
            agent.speed = 0;
        }
        else
        {
            //body.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            //agent.SetDestination(player.transform.position);
            //agent.speed = 0;
            agent.speed = anger;
        }

        agent.SetDestination(player.transform.position);

        if (Time.time - timeSinceStunned > stunDuration) // Stop being stunned after duration ends
        {
            stunned = false;
        }

        // Kill player if touching them
        if (Vector3.Distance(transform.position, player.transform.position) <= 1 && anger > 0)
        {
            player.Die();
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

    // Gets faster
    public void PissOff()
    {
        //if (anger < maxAnger)
        //{
        //    anger++;
        //    agent.speed = anger;
        //}
        anger += 2;
        agent.speed = anger;
    }

    // Getting hit by player flashlight stuns statue for 2 sec
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player Flashlight") && !Physics.Linecast(transform.position, player.transform.position, layerMask))
        {
            Debug.Log("STUNNED");
            stunned = true;
            timeSinceStunned = Time.time;
        }
    }
}
