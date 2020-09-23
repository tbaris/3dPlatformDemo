using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.current.startRun += cursorLockAndHide;
        transform.GetComponent<CinemachineBrain>().enabled = false;
    }


    private void cursorLockAndHide() //locks and hides cursor at start signal and enables camera controls 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.GetComponent<CinemachineBrain>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape)) // sets free the cursor when escape hitted
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
            
    }
    private void OnDestroy()
    {
        GameManager.current.startRun -= cursorLockAndHide;
        
    }
    
}
