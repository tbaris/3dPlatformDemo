using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public TextMeshProUGUI countDownText;
    public TMP_Dropdown returnMode;
    public TMP_InputField opponentCount;
    public Button StartButton;
    public Button reloadButton;
    public int countFrom = 3;
    private bool isCountDownStarted = false;
    public GameObject startPanel;
    public Toggle canJump;
   

    void Awake()
    {
        current = this;
    }

    private void Start()
    {
        StartButton.onClick.AddListener(delegate { startCountDown(countFrom); }); //-set button func to startcountdown
        reloadButton.onClick.AddListener(delegate { reloadScene(); }); // - set button func to reloadScene
    }

    

    public void startCountDown(int countFrom)  //sets game props and starts countdown
    {
       
        if(opponentCount.text != "") // if empty spawn 10 opponents
        {
            spawnGirls(int.Parse(opponentCount.text));
        }
        else
        {
            spawnGirls(10);
        }

        startPanel.gameObject.SetActive(false);
        
        
        setJumpModeFunc(canJump.isOn);
        StartCoroutine("CountDown",countFrom);
    }

    IEnumerator CountDown(int startInt) // countdown from given time as seconds and gives start signal
    {
        if(isCountDownStarted)
        {
            yield break;
        }
        isCountDownStarted = true;
       
        countDownText.text = startInt.ToString();
        countDownText.gameObject.SetActive(true);
        while (startInt > 0)
        {
            yield return new WaitForSeconds(1);
            startInt--;
            countDownText.text = startInt.ToString();
        }
       
        countDownText.gameObject.SetActive(false);
        setReturnModeFunc(returnMode.value);
        startSignal();
        
        isCountDownStarted = false;
    }

    public event Action startRun;

    private void startSignal()//start signal to enable contestants control at the time
    {
        if(startRun != null)
        {
            startRun();
        }
    }

    public event Action<int> spawnOpponents;
    private void spawnGirls(int amount) //signals to spawner to spawn opponents
    {
        if (spawnOpponents != null)
        {
            spawnOpponents(amount);
        }
    }

    public event Action<int> setReturnMode;// event to set return mode as desciribed at ui

    private void setReturnModeFunc(int mode) 
    {
        if (setReturnMode != null)
        {
            setReturnMode(mode);
        }
    }

    public event Action<bool> setJumpMode; // enable or disable jump event

    private void setJumpModeFunc(bool mode) 
    {
        if (setJumpMode != null)
        {
            setJumpMode(mode);
        }
    }

    private void reloadScene() //reloads current scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



}
