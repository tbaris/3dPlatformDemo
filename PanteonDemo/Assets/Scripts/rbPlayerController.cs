using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rbPlayerController : MonoBehaviour
{
   
    Transform cam;

    public bool isPlayer = false;

    private NavMeshAgent agent;
    public Transform targetTransform;

    public Vector3 startPos;
    public Quaternion startRot;

    public float speed = 10f;    //player speed
    private float smoothingTurn = 0;    
    public float turnSmoothTime = 0.1f;  //player rotation smoothing
    Animator animator;//player animator 
    Rigidbody rb;//player rigidbody
    
    Vector3 moveDirection = Vector3.zero;
    Vector3 camDirection = Vector3.zero;

    public float groundDetectionDistance = 0.1f;
    public bool isGrounded = false;
    public bool onPlatform = false;
    public bool jumped = false;
    public float jumpPower = 500;
    public float jumpAnimDelay = 0.25f;
    public float jumpReloadTime = 1f;

    public bool isHitted = false;

    public Vector3 externalDrag;
    private float startAgentSpeed;

    

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (isPlayer)
        {
            cam = Camera.main.transform;
        }
        else
        {
            agent = transform.GetComponent<NavMeshAgent>();
            agent.destination = targetTransform.position;
            agent.updateRotation = false;
            agent.updatePosition = false;
            startAgentSpeed = agent.speed;

        }

    }

   
    void Update()
    {

        if (isPlayer)
        {
            //rotate player model acording to camera
            float targetAngle = cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothingTurn, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //move player model acording to camera
            camDirection = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            moveDirection = Input.GetAxis("Vertical") * camDirection + Input.GetAxis("Horizontal") * cam.right;
            moveDirection = moveDirection.normalized * speed;
            animator.SetBool("isGrounded", isGrounded);

            if (Input.GetAxis("Jump") > 0 && isGrounded && !jumped)
            {
                animator.SetTrigger("Jumped");
                jumped = true;
                StartCoroutine("JumpReload");
            }
        }
        else
        {
            float agentDistance = (agent.nextPosition - transform.position).magnitude;
            if (agentDistance > 2)
            {
                agent.nextPosition = transform.position; 
            }
            else if(agentDistance > 1)
            {
                agent.speed = 0.1f;
            }
            else
            {
                agent.speed = startAgentSpeed;
            }
            /* float targetAngle = Vector3.Angle(agent.velocity.normalized, -this.transform.forward);
             if (agent.velocity.normalized.y < this.transform.forward.y)
             {
                 targetAngle *= -1;
             }
             targetAngle = (targetAngle + 180.0f) % 360.0f;
             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothingTurn, turnSmoothTime);
             //transform.rotation = Quaternion.Euler(0f, angle, 0f);
             moveDirection = agent.desiredVelocity;*/
            transform.LookAt (new Vector3(agent.nextPosition.x, transform.position.y, agent.nextPosition.z));
            moveDirection = (agent.nextPosition - transform.position).normalized * speed;
            animator.SetBool("isGrounded", isGrounded);
            

        }
        
        
       
    }

    IEnumerator JumpReload()
    {
        yield return new WaitForSeconds(jumpAnimDelay);
        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(jumpReloadTime);
        jumped = false;
    }

    public void Hitted()
    {
        isHitted = true;
        StartCoroutine("HittedTimer");
    }

    IEnumerator HittedTimer()
    {
        
        yield return new WaitForSeconds(jumpReloadTime);
        isHitted = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Platform")
        {
            onPlatform = true;
        }else if (collision.transform.tag == "FallDetect")
        {
            transform.position = startPos;
            transform.rotation = startRot;

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Platform")
        {
           onPlatform = false;
        }
    }

    void FixedUpdate()
    {
        if(!Physics.Raycast(transform.position,-Vector3.up, groundDetectionDistance ) && !onPlatform)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
            if (!isHitted)
            {
                rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z) + externalDrag;
            }
        }

      
        Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.velocity- externalDrag);
        animator.SetFloat("AnimHor", localVelocity.x);
        animator.SetFloat("AnimVer", localVelocity.z);
    }

}
