using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    public float movementSpeed;
    private Rigidbody2D playerRb;

    private Vector3 moveInputDirection;
    private Vector2 moveVelocity;
    private Vector2 aimInputDirection;
    private Vector2 bulletDirection;
    private Vector2 fireDirection;
    private Vector2 weaponDirection;


    public GameObject aimingPoint;
    public GameObject bulletPrefab;
    public GameObject dashTrail;
    public GameObject firePoint;
    public GameObject player;

    private Animator playerAnimator;

  
    private bool canMove = true;
    private bool canDash = true;
    private bool canShoot = true;

    public float dashForce;
    public float timeBetweenShoot;
    public float dashDuration;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        canMove = true;
    }


    void Update()
    {
       

        moveInputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0.0f);
        moveVelocity = moveInputDirection * movementSpeed;
       
        bulletDirection = new Vector2(Input.GetAxisRaw("HorizontalSecondJoystick"), Input.GetAxisRaw("VerticalSecondJoystick"));
        fireDirection = new Vector3(Input.GetAxisRaw("HorizontalSecondJoystick") * 1f, Input.GetAxisRaw("VerticalSecondJoystick") * 1f);
        weaponDirection = new Vector3(Input.GetAxisRaw("HorizontalSecondJoystick") * 0.01f, Input.GetAxisRaw("VerticalSecondJoystick") * 0.01f);
        firePoint.transform.localPosition = fireDirection;
        AimAndShoot();
        

        if (Input.GetButtonDown("Dash") && canMove == true && canDash == true)
        {
            StartCoroutine(Dash());
        }
       

    }

    private void FixedUpdate()
    {

        if (canMove == true)
        {
            playerRb.velocity = moveVelocity;    
        }
    }


    void AimAndShoot()
    {
       Vector3 aimInputDirection = new Vector3(Input.GetAxisRaw("HorizontalSecondJoystick"), Input.GetAxisRaw("VerticalSecondJoystick"));

        bulletDirection.Normalize();
        if (aimInputDirection.magnitude > 0.0f)
        {

            aimInputDirection.Normalize();
            aimingPoint.transform.localPosition =  aimInputDirection;
            aimingPoint.SetActive(true);
        }
        else
        {
            aimingPoint.SetActive(false);
        }
        if (Input.GetButton("Fire") && aimInputDirection != Vector3.zero)
        {
            if (canShoot)
            {
                canShoot = false;
                Shoot();
                StartCoroutine(TimeBetween());
            }

        }
    }


    IEnumerator Dash()
    {
        playerRb.velocity = moveInputDirection.normalized * dashForce;
        dashTrail.SetActive(true);
        canMove = false;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        
        canMove = true;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
        dashTrail.SetActive(false);
    }

    IEnumerator TimeBetween()
    {       
            yield return new WaitForSeconds(timeBetweenShoot);
            canShoot = true;
    }

    void Shoot()
    {
        float bulletSpeed = 20f;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
        bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg);
        Destroy(bullet, 2.0f);
    }



}
