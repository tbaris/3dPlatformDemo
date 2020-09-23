using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour
{

    private Vector3 dragFactor;
    public Vector3 rotationAngle = Vector3.forward;
    public float rotationSpeed = 8f;
    private float activeRotationSpeed = 0;

    public bool rotateRandomly = false;
    public bool dragOpponents = false;

    // Start is called before the first frame update
    void Start()
    {
        if(rotateRandomly)
        {
            StartCoroutine("RotateRandom");
        }
        else
        {
            activeRotationSpeed = rotationSpeed;
        }
    }

    IEnumerator RotateRandom()
    {
        yield return new WaitForSeconds(Mathf.Abs(activeRotationSpeed/4)); //waits as long as rotation speed
        if (activeRotationSpeed > 0)
        {
            activeRotationSpeed = -Random.Range(3, rotationSpeed); //reverse
        }
        else
        {
            activeRotationSpeed = Random.Range(3, rotationSpeed);//forward
        }
        
        StartCoroutine("RotateRandom");
    }

    // Update is called once per frame
    void Update()
    {
       
            transform.Rotate(rotationAngle * activeRotationSpeed * Time.deltaTime);//rotates object
            dragFactor = rotationAngle * activeRotationSpeed / 2; // sets applied force to oppenents
        
    }


    private void OnCollisionEnter(Collision other) //applies force to opponents on rotation vector
    {

        if ((other.transform.tag == "Player" || other.transform.tag == "Opponent") && dragOpponents)
        {

            other.transform.GetComponent<rbPlayerController>().externalDrag = dragFactor;
        }
    }

    private void OnCollisionStay(Collision other)//applies force to opponents on rotation vector
    {
        if ((other.transform.tag == "Player" || other.transform.tag == "Opponent") && dragOpponents)
        {

            other.transform.GetComponent<rbPlayerController>().externalDrag = dragFactor;
        }
    }

    private void OnCollisionExit(Collision other)// resets aplied force when leaved
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Opponent")
        {
            other.transform.GetComponent<rbPlayerController>().externalDrag = Vector3.zero;

        }
    }
}
