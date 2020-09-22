using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePunch : MonoBehaviour
{
    public float punchPower = 1000f;

    public float punchScaleFactor = 1.1f;

    public bool punchAnimOver = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.transform.tag == "Player" || collision.transform.tag == "Opponent") && !collision.transform.GetComponent<rbPlayerController>().isHitted) 
        {
            Debug.Log("player hit");
            
            Vector3 hitDirection = Vector3.Reflect(collision.contacts[0].point-collision.transform.position, collision.contacts[0].normal);
            if (punchAnimOver)
            {
                punchAnimOver = false;
                transform.DOPunchScale(transform.localScale * punchScaleFactor, 0.2f, 2, 1);
            }

            collision.transform.GetComponent<rbPlayerController>().Hitted();
            collision.transform.GetComponent<Rigidbody>().AddForce(hitDirection.normalized * punchPower, ForceMode.Impulse);
            collision.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * punchPower/10, ForceMode.Impulse);
        }
    }

    IEnumerator punchAnimReload()
    {
        yield return new WaitForSeconds(0.3f);
        punchAnimOver = true;
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
