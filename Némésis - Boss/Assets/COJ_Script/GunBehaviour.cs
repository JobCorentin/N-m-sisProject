using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public Transform aimingPoint;
    public Rigidbody2D rb;
    public GameObject weaponPointPosition;

    Vector2 aimingPointPos;

    void Start()
    {
        
    }


    void Update()
    {
        aimingPointPos = aimingPoint.transform.position;
        Vector2 lookDir = aimingPointPos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        rb.position = weaponPointPosition.transform.position;

    }
}
