using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nightguard : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private AudioManager audioManager;

    // Patrolling variables
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet = false;



    [SerializeField] private NightguardFlashlight flashlight;
    [SerializeField] private float investigationDuration; // how long to spend investigating
    private float timeSinceDetection; // When was the player last seen?


    private float timeSinceStoppedMoving;
    private float loiterDelay;
    private Rigidbody body;
    private bool walkFlag = false;
    private bool detectedFlag = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (flashlight.playerDetected) // When player is first seen in flash light
        {

            if (!Physics.Linecast(transform.position, player.transform.position, flashlight.mask)) // Chase while still seen
            {
                Chase();
                if (!detectedFlag && !audioManager.IsPlaying("I SEE YOU"))
                {
                    audioManager.Play("I SEE YOU");
                    detectedFlag = true;
                }
                //Debug.Log("CHASING");
            }
            else if (Time.time - timeSinceDetection <= investigationDuration) // Go to last known location
            {
                Investigate();
                detectedFlag = false;
                //Debug.Log("INVESTIGATING");
            }
            else // Resume patrolling if player got away
            {
                audioManager.Play("dang it");
                flashlight.playerDetected = false;
                detectedFlag = false;
            }
        }
        else
        {
            Patrol();
            //Debug.Log("PATROLLING");
        }

        HandleMovementStopped();
    }

    private void Chase()
    {
        walkPoint = player.transform.position; // Gets current location of player
        timeSinceDetection = Time.time; // Gets current time;
        transform.LookAt(walkPoint);
        agent.SetDestination(walkPoint);
    }

    private void Investigate()
    {
        transform.LookAt(walkPoint);
        agent.SetDestination(walkPoint);
    }

    private void Patrol()
    {
        //if (!player.detected)
        //{
        //    if (!walkPointSet)
        //    {
        //        FindWalkPoint();
        //    }
        //    else
        //    {
        //        agent.SetDestination(walkPoint);
        //    }

        //    //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //}
        //else // If player seen in light, move towards where player was first seen in spotlight
        //{

        //    //if (!detectedFlag)
        //    //{
        //    //    detectedFlag = true;
        //    //    walkPoint = player.transform.position;
        //    //}
        //    //else
        //    //{
        //    //    agent.SetDestination(player.transform.position);
        //    //}
        //    agent.SetDestination(player.transform.position);

        //}


        if (!walkPointSet)
        {
            FindWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

    }

    private void FindWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) // Double checks if spot is on ground
        {
            walkPointSet = true;
        }


        
    }

    private void HandleMovementStopped()
    {
        if (body.velocity.x < 1 && body.velocity.z < 1) // When not moving, move again after random time
        {

            if (!walkFlag)
            {
                walkFlag = true;
                loiterDelay = Random.Range(0.25f, 10f);
                timeSinceStoppedMoving = Time.time;
            }
            else if (Time.time - timeSinceStoppedMoving >= loiterDelay)
            {
                walkFlag = false;
                walkPointSet = false;
            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Flashlight") && !Physics.Linecast(transform.position, player.transform.position, flashlight.mask))
        {
            flashlight.playerDetected = true;
            Debug.Log("Hit by player flash");
        }
    }
}
