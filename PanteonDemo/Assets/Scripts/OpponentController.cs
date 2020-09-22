using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : MonoBehaviour
{

    private Rigidbody rb;
    private NavMeshAgent agent;
    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        agent = transform.GetComponent<NavMeshAgent>();
        agent.destination = targetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = agent.desiredVelocity;
    }
}
