using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rbPlayerController : MonoBehaviour
{
   
    Transform cam;

    public bool isPlayer = false;

    private NavMeshAgent agent;
    public GameObject targetTransform;
    public int wayPointIndex = 0;

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

    private bool jumpMode;
    private bool isRunStarted = false;

    private int returnStartMode  = 0; //0-means return start when hit. 1-Means return start only fall to the sea

    

    void Start()
    {
        //subscribe events
        GameManager.current.startRun += enableControl;
        GameManager.current.setReturnMode += setReStartMode;
        GameManager.current.setJumpMode += setJumpBool;

        //saves pos an rot at start to return same pos and rot when needed
        startPos = transform.position;
        startRot = transform.rotation;


        targetTransform = GameObject.FindGameObjectWithTag("Finish");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (isPlayer)//sets cam if player
        {
            cam = Camera.main.transform;
        }
        else//sets navmesh agent if ai
        {
           
            agent = transform.GetComponent<NavMeshAgent>();
            agent.destination = targetTransform.transform.GetComponent<WaypointHolder>().wayPoints[wayPointIndex].gameObject.transform.position;
            agent.updateRotation = false;
            agent.updatePosition = false;
            startAgentSpeed = agent.speed;

        }

    }

    private void setReStartMode(int mode) //sets return opponents mode when event called
    {
        returnStartMode = mode;

    }

    private void setJumpBool(bool mode)//sets jump mode when event called
    {
        jumpMode = mode;
    }

    private void OnDestroy()
    {

        //unsubscribe events
        GameManager.current.startRun -= enableControl;
        GameManager.current.setReturnMode -= setReStartMode;
        GameManager.current.setJumpMode -= setJumpBool;
    }

    private void enableControl()// enable controls when start event called
    {
        isRunStarted = true;
    }

   
    void Update()
    {
        if (isRunStarted)
        {

            if (isPlayer)// if it is player sets control to axis and cam rotation
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

                if (Input.GetAxis("Jump") > 0 && isGrounded && !jumped && jumpMode) //jump machanics
                {
                    animator.SetTrigger("Jumped");
                    jumped = true;
                    StartCoroutine("JumpReload");
                }
            }
            else// if its ai sets controls to follow navmesh agent
            {
                float agentDistance = (agent.nextPosition - transform.position).magnitude;


                if (agentDistance > 2.1f)// resets navmesh agent position if gets too far away
                {
                    agent.nextPosition = transform.position;
                }
                else if (agentDistance > 0.5f)// slow down navmesh agent if gets far
                {
                    agent.speed = Mathf.Lerp(startAgentSpeed, 0.1f, agentDistance / 2);

                }
                else
                {
                    agent.speed = startAgentSpeed;
                }

                transform.LookAt(new Vector3(agent.nextPosition.x, transform.position.y, agent.nextPosition.z)); //sets rotation to look to navmesh agent
                moveDirection = (agent.nextPosition - transform.position).normalized * speed * agentDistance; //sets direction towards to navmesh agent
                animator.SetBool("isGrounded", isGrounded);




                foreach (KeyValuePair<GameObject, float> keyValue in targetTransform.transform.GetComponent<WaypointHolder>().wayPointDict)
                {

                    if (Vector3.Distance(transform.position, targetTransform.transform.position) - 3 > keyValue.Value)
                    {
                        agent.destination = keyValue.Key.transform.position;
                        break;
                    }
                    else
                    {
                        agent.destination = targetTransform.transform.position;
                    }
                }




                /*
                if (Vector3.Distance(agent.destination, transform.position) < 1) //sets navmesh agent destination to next waypoint if current waypoint is near 
                {
                    
                    if (wayPointIndex + 1 < targetTransform.transform.GetComponent<WaypointHolder>().wayPoints.Count)
                    {
                        wayPointIndex = wayPointIndex + 1;
                        agent.destination = targetTransform.transform.GetComponent<WaypointHolder>().wayPoints[wayPointIndex].gameObject.transform.position;
                    }
                    else // if waypoints ended sets to final destination
                    {
                        agent.destination = targetTransform.transform.position;
                    }
                }*/


            }


        }
    }

    IEnumerator JumpReload()//jump reload coroutine to prevent flying
    {
        yield return new WaitForSeconds(jumpAnimDelay);
        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(jumpReloadTime);
        jumped = false;
    }

    public void Hitted() // if player hitted disables control
    {
        isHitted = true;

        StartCoroutine("HittedTimer" , 1f);
    }

    IEnumerator HittedTimer(float waitToReturn)// gives player contol back and if requested return player back in start pos.
    {
        
        yield return new WaitForSeconds(waitToReturn);
        isHitted = false;
        if(returnStartMode == 0)
        {
            returnStart();
        }
    }

    private void returnStart()// returns opponents to saved start pos and rot. 
    {
        transform.position = startPos;
        transform.rotation = startRot;
        wayPointIndex = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if(!isPlayer)
        {
            agent.enabled = false;
            
            rb.isKinematic = true;
            isRunStarted = false;

        }
        

        StartCoroutine("returnCoolDown", 0.5f); 

    }
    IEnumerator returnCoolDown(float coolDownTime)// gives player contol back and if requested return player back in start pos.
    {

        yield return new WaitForSeconds(coolDownTime);
        agent.enabled = true;
       
        isRunStarted = true;
        rb.isKinematic = false;
        agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision collision) //checks if char on platform or falled to the sea
    {
        if(collision.transform.tag == "Platform")
        {
            onPlatform = true;
        }else if (collision.transform.tag == "FallDetect")
        {
            returnStart(); // if falled returns back to start

        }
    }
    private void OnCollisionExit(Collision collision) //checks if char not on platform to disable control
    {
        if (collision.transform.tag == "Platform")
        {
           onPlatform = false;
        }
    }

    void FixedUpdate()
    {
        if(!Physics.Raycast(transform.position,-Vector3.up, groundDetectionDistance ) && !onPlatform) // if char on platform applies requested velocities to rigidbody
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
            if (!isHitted && isRunStarted)//and if not hitted
            {
                rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z) + externalDrag;
            }
        }

      
        Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.velocity- externalDrag); //adds external drag (rotating platforms etc.)

        //sets animation values for blend tree
        animator.SetFloat("AnimHor", localVelocity.x);
        animator.SetFloat("AnimVer", localVelocity.z);
    }

}
