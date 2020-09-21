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
        yield return new WaitForSeconds(Mathf.Abs(activeRotationSpeed));
        if (activeRotationSpeed > 0)
        {
            activeRotationSpeed = -Random.Range(3, rotationSpeed);
        }
        else
        {
            activeRotationSpeed = Random.Range(3, rotationSpeed);
        }
        
        StartCoroutine("RotateRandom");
    }

    // Update is called once per frame
    void Update()
    {
       
            transform.Rotate(rotationAngle * activeRotationSpeed * Time.deltaTime);
            dragFactor = rotationAngle * activeRotationSpeed / 2;
        
    }


    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {

            other.transform.GetComponent<rbPlayerController>().externalDrag = dragFactor;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {

            other.transform.GetComponent<rbPlayerController>().externalDrag = dragFactor;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<rbPlayerController>().externalDrag = Vector3.zero;

        }
    }
}
