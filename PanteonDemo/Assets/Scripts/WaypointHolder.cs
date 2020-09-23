using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointHolder : MonoBehaviour
{
    public List<GameObject> wayPoints;
   
    public Dictionary<GameObject, float> wayPointDict;


    // Start is called before the first frame update
    void Start()
    {
        wayPointDict = new Dictionary<GameObject, float>();
        //gets waypoints and sets in order(from far to near)
        wayPoints.Clear();
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
        wayPoints = wayPoints.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
        wayPoints.Reverse();

        List<GameObject> WP = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        WP = WP.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
        WP.Reverse();

        for (int i = 0; i < wayPoints.Count; i++)
        {
            wayPointDict.Add(wayPoints[i], Vector3.Distance(transform.position, wayPoints[i].transform.position));
        }








    }


}
