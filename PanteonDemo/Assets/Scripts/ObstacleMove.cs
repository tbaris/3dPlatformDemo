using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public bool dirRight = true;

    public float moveDuration = 1;
    public float zMoveRange = 3;
    public float xMoveRange = 3;

    private float zMinPoint;
    private float zMaxPoint;

    private float xMinPoint;
    private float xMaxPoint;

    void Start()
    {
        zMinPoint = transform.position.z - zMoveRange;
        zMaxPoint = transform.position.z + zMoveRange;

        xMinPoint = transform.position.x - xMoveRange;
        xMaxPoint = transform.position.x + xMoveRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (dirRight)
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.DOMove(new Vector3 (xMaxPoint + 0.1f ,transform.position.y ,zMaxPoint + 0.1f), moveDuration);
        }
        else
        {
            transform.DOMove(new Vector3(xMinPoint - 0.1f ,transform.position.y ,zMinPoint - 0.1f), moveDuration);
        }
        if (transform.position.z >= zMaxPoint && transform.position.x >= xMaxPoint)
        {
            dirRight = false;
        }

        if (transform.position.z <= zMinPoint && transform.position.x <= xMinPoint)
        {
            dirRight = true;
        }
    }
}
