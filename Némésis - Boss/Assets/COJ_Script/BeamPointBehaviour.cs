using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPointBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject player;
    public Vector3 dir;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float pointSpeed = 2.5f;
        dir = player.transform.position - rb.transform.position;
        rb.velocity = dir * pointSpeed;
    }
}
