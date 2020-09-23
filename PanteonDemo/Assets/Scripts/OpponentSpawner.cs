using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSpawner : MonoBehaviour
{
    public GameObject opponent;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.current.spawnOpponents += SpawnOpponents;
    }

    public void SpawnOpponents(int amount) // spawn opponents when countdown event called 
    {
        for (int i = 0; i < amount; i++)
        {
            if (i % 2 == 0)
            {
              GameObject girl = Instantiate(opponent, new Vector3(-40 + ((i / 12) * 2), 0.3f, (((i % 12) / 2)+1) * (22.0f/12)), Quaternion.Euler(0, 90, 0)) as GameObject;
                girl.transform.parent = transform;

            }
            else
            {
                GameObject girl = Instantiate(opponent, new Vector3(-40+((i/12)*2), 0.3f, (((i%12)/2)+1) * -(22.0f/12)), Quaternion.Euler(0, 90, 0)) as GameObject;
                girl.transform.parent = transform;
            }
        }
       
    }

    private void OnDestroy()
    {
        GameManager.current.spawnOpponents -= SpawnOpponents;
    }
}
