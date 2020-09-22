﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public GameObject CineCamContoroller;
    public GameObject PaintPivot;
    public WallPainter WP;
    // Start is called before the first frame update
    void Start()
    {
        WP = Camera.main.GetComponent<WallPainter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.GetComponent<rbPlayerController>().enabled = false;
        collision.transform.GetComponent<Animator>().SetFloat("AnimHor", 0);
        collision.transform.GetComponent<Animator>().SetFloat("AnimVer", 0);
        CineCamContoroller.GetComponent<CinemachineFreeLook>().Follow = PaintPivot.transform;
        CineCamContoroller.GetComponent<CinemachineFreeLook>().LookAt = PaintPivot.transform;
        WP.enabled = true;
        CineCamContoroller.GetComponent<CinemachineCollider>().enabled = false;
    }
}