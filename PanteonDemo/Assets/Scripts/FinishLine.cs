using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class FinishLine : MonoBehaviour
{

    public GameObject CineCamContoroller;
    public GameObject PaintPivot;
    public WallPainter WP;
    public List<GameObject> finishedContestants;

    private bool isStarted = false;
    public List<GameObject> contestants;
    
    // Start is called before the first frame update
    void Start()
    {
        WP = Camera.main.GetComponent<WallPainter>();

        GameManager.current.startRun += getContestants;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            getPosition();
        }
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
    public void getPosition()
    {


        contestants = contestants.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();

        foreach (GameObject op in contestants)
        {
            op.transform.GetComponentInChildren<TextMeshPro>().text = showPosition(contestants.IndexOf(op) + 1 + finishedContestants.Count);
        }



    }
    private string showPosition(int positionNumer)
    {
        switch (positionNumer)
        {

            case 3:
                return (positionNumer.ToString() + "rd/" + (contestants.Count + finishedContestants.Count).ToString());

            case 2:
                return (positionNumer.ToString() + "nd/" + (contestants.Count+ finishedContestants.Count).ToString());

            case 1:
                return (positionNumer.ToString() + "st/" + (contestants.Count+ finishedContestants.Count).ToString());

            default:
                return (positionNumer.ToString() + "th/" + (contestants.Count+ finishedContestants.Count).ToString());

        }
    }

    private void OnDestroy()
    {
        GameManager.current.startRun -= getContestants;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") // if player reaches the fnish disable 3rd person controls , sets camera for wall paint  event and starts wall paint mechanics
        {
            collision.transform.GetComponent<rbPlayerController>().enabled = false;
            collision.transform.GetComponent<Animator>().SetFloat("AnimHor", 0);
            collision.transform.GetComponent<Animator>().SetFloat("AnimVer", 0);
            CineCamContoroller.GetComponent<CinemachineFreeLook>().Follow = PaintPivot.transform;
            CineCamContoroller.GetComponent<CinemachineFreeLook>().LookAt = PaintPivot.transform;
            WP.enabled = true;
       
            CineCamContoroller.GetComponent<CinemachineCollider>().enabled = false; // disables cam collider to prevent erratic behavior
            collision.transform.GetComponent<Collider>().enabled = false;
            collision.transform.GetComponent<Rigidbody>().isKinematic = true;

            contestants.Remove(collision.gameObject);
            finishedContestants.Add(collision.gameObject);
            int pos = finishedContestants.IndexOf(collision.gameObject)+1;

            collision.transform.GetComponentInChildren<TextMeshPro>().text =showPosition(pos);

            placeFinished(collision.gameObject, pos);

            

        }
        else if(collision.transform.tag == "Opponent") // if ai reaches the finish disables controls and stop navmesh agent
        {
            collision.transform.GetComponent<rbPlayerController>().enabled = false;
            collision.transform.GetComponent<Animator>().SetFloat("AnimHor", 0);
            collision.transform.GetComponent<Animator>().SetFloat("AnimVer", 0);
            collision.transform.GetComponent<NavMeshAgent>().isStopped = true;
            
            collision.transform.GetComponent<Collider>().enabled = false;
            collision.transform.GetComponent<Rigidbody>().isKinematic = true;

            finishedContestants.Add(collision.gameObject);
            contestants.Remove(collision.gameObject);
            int pos = finishedContestants.IndexOf(collision.gameObject) +1;

            collision.transform.GetComponentInChildren<TextMeshPro>().text = showPosition(pos);

            placeFinished(collision.gameObject, pos);

        }
        
    }

    private void placeFinished(GameObject Finished , int i)
    {
        i--;
        Finished.transform.position = new Vector3(188 + ((i / 10) * 1.4f), 0.3f, -7 + (i % 10) * 1.5f);
             
        Finished.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}
