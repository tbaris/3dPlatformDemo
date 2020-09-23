using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public bool startDirBack = true;

    public bool waitOnForwardDir = false;
    public bool waitOnBackDir = false;

    public float moveDuration = 1;
    public float zMoveRange = 3;
    public float xMoveRange = 3;

    private float zMinPoint;
    private float zMaxPoint;

    private float xMinPoint;
    private float xMaxPoint;

    public bool isRandomIntervals = false;
   
    public float minWait = 0.5f;
    public float maxWait = 5f;

    

    void Start()
    {
        
        zMinPoint = transform.position.z - zMoveRange;
        zMaxPoint = transform.position.z + zMoveRange;

        xMinPoint = transform.position.x - xMoveRange;
        xMaxPoint = transform.position.x + xMoveRange;
        if (!startDirBack)
        {
            StartCoroutine("WaitAndGo");
        }
        else { StartCoroutine("WaitAndGoBack"); }
    }


      
    IEnumerator WaitAndGo()
    {
        if (waitOnForwardDir)//waits if setted
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
        }
        transform.DOMove(new Vector3(xMaxPoint + 0.1f, transform.position.y, zMaxPoint + 0.1f), moveDuration).OnComplete(() => StartCoroutine("WaitAndGoBack"));

    }
    IEnumerator WaitAndGoBack()
    {
        if (waitOnBackDir)//waits if setted
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
        }
        transform.DOMove(new Vector3(xMinPoint - 0.1f, transform.position.y, zMinPoint - 0.1f), moveDuration).OnComplete(() => StartCoroutine("WaitAndGo"));

    }

}
