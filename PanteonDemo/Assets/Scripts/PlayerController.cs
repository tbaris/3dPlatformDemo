using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController CharCont;
    public float speed;
    public float turnSmoothTime = 0.05f;
    float smoothingVel;
    public Transform cam;
    public float gravity = 6;
    public float jumpHeight = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 gravityPull = new Vector3(0f, -gravity * Time.deltaTime, 0f);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        Debug.Log(gravityPull);
        if (Input.GetButtonDown("Jump") /*&& _isGrounded*/)
        {
            Debug.Log("jumped"); 
            gravityPull = new Vector3(0f, Mathf.Sqrt(jumpHeight * -2f * gravity ), 0f);
            Debug.Log(Mathf.Sqrt(jumpHeight * -2f * gravity));
        }
           
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y ;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothingVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);



            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            

            CharCont.Move((moveDirection.normalized * speed * Time.deltaTime)+ gravityPull);
        }else
        {
            CharCont.Move(gravityPull);
        }
        
    }
}
