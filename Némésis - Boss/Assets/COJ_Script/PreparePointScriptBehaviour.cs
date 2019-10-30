using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePointScriptBehaviour : MonoBehaviour
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
            dir = player.transform.position - impactPointRb.transform.position + new Vector3 (0,1.5f,0);
            impactPointRb.velocity = dir * absoluteSpeed;
        }

    }

    IEnumerator timeBeforeComingBack()
    {
        Debug.Log("Points behind you");
        dir = new Vector3(0, 0, 0);
        impactPointRb.velocity = dir;
        canDo = false;
        yield return new WaitForSeconds(0.5f);
        canDo = true;

    }
}
