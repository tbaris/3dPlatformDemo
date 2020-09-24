using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class ShowPosition : MonoBehaviour
{

    private bool isStarted = false;
    public List<GameObject> contestants;
    public GameObject finishLine;
    // Start is called before the first frame update
    void Start()
    {

        GameManager.current.startRun += getContestants;
        finishLine = GameObject.FindGameObjectWithTag("Finish");
        

    }

    private void getContestants() // gets all contestant after opponents spwaned
    {
        contestants.Clear();
        contestants.AddRange(GameObject.FindGameObjectsWithTag("Opponent"));
        contestants.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        if (contestants.Count > 0)
        {
            isStarted = true;
        }
    }

    private void Update()
    {
        if(isStarted)
        {
            getPosition();
        }
    }


    public void getPosition()
    {
       
            
            contestants = contestants.OrderBy(x => Vector3.Distance(finishLine.transform.position, x.transform.position)).ToList();

            foreach (GameObject op in contestants)
            {
                 op.transform.GetComponentInChildren<TextMeshPro>().text = showPosition(contestants.IndexOf(op) + 1 + finishLine.transform.GetComponent<FinishLine>().finishedContestants.Count);
            }

     
        
    }
    private string showPosition(int positionNumer)
    {
        switch (positionNumer)
        {

            case 3:
                return (positionNumer.ToString() + "rd/" + contestants.Count);
                
            case 2:
                return (positionNumer.ToString() + "nd/" + contestants.Count);
                
            case 1:
                return (positionNumer.ToString() + "st/" + contestants.Count);
               
            default:
                return (positionNumer.ToString() + "th/" + contestants.Count);
                
        }
    }

    private void OnDestroy()
    {
        GameManager.current.startRun -= getContestants;
    }
}
