using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrol : MonoBehaviour
{
    public Animator animator;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        //since the enemy is always moving I have them always walking
        animator.SetFloat("Speed", 1);
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance == 0)
            GotoNextPoint();

        if(agent.transform.rotation.eulerAngles.y >=0 && agent.transform.rotation.eulerAngles.y <= 179)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, -agent.transform.rotation.y, 0 );
            spriteRenderer.flipX = false;
        }
        if(agent.transform.rotation.eulerAngles.y > 179)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, -agent.transform.rotation.y, 0);
            spriteRenderer.flipX = true;
        }


    }
}
