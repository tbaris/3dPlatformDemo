using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class ShowPosition : MonoBehaviour
{
    public TextMeshPro positionText;
    public List<GameObject> contestants;
    // Start is called before the first frame update
    void Start()
    {

        GameManager.current.startRun += getContestants;
       
    }

    private void getContestants() // gets all contestant after opponents spwaned
    {
        contestants.Clear();
        contestants.AddRange(GameObject.FindGameObjectsWithTag("Opponent"));
        contestants.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (contestants.Count > 0)
        {
            //sort contestants by the distance to the finish line
            contestants = contestants.OrderBy(x => Vector3.Distance(transform.gameObject.GetComponent<rbPlayerController>().targetTransform.transform.position, x.transform.position)).ToList();

            int positionNumer = contestants.IndexOf(this.gameObject) + 1;

            //shows rank of player as ordinal numbers

            switch (positionNumer)
            {

                case 3:
                    positionText.text = positionNumer.ToString() + "rd/" + contestants.Count;
                    break;
                case 2:
                    positionText.text = positionNumer.ToString() + "nd/" + contestants.Count;
                    break;
                case 1:
                    positionText.text = positionNumer.ToString() + "st/" + contestants.Count;
                    break;
                default:
                    positionText.text = positionNumer.ToString() + "th/" + contestants.Count;
                    break;
            }
        }

       
    }

    private void OnDestroy()
    {
        GameManager.current.startRun -= getContestants;
    }
}
