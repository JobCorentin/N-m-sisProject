using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPointBehaviour : MonoBehaviour
{
    public Rigidbody2D impactPointRb;
    public GameObject player;
    public Vector3 dir;
    protected bool canDo = true;
    // Update is called once per frame
    void Update()
    {
        float absoluteSpeed = 15f;

        if (Input.GetButtonDown("Dash"))
        {

            StartCoroutine(timeBeforeComingBack());
            Debug.Log("you dashed");
        }
        else if (canDo == true)
        {
            dir = player.transform.position - impactPointRb.transform.position;
            impactPointRb.velocity =  dir* absoluteSpeed;
        }
        
    }

    IEnumerator timeBeforeComingBack()
    {
        Debug.Log("Points behind you");
        dir = new Vector3(0, 0, 0);
        impactPointRb.velocity = dir;
        canDo = false;
        yield return new WaitForSeconds(0.3125f);
        canDo = true;

    }
}
