using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public GameObject player;
    protected int playerHealth = 100;
    protected int damageTaken;
    protected bool canTakeDamage = true;
    protected float privateTimer;
    public Slider playerBar;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerBar.value = playerHealth;

        if(playerHealth <= 0)
        {
            Destroy(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bossLeftArm01") && canTakeDamage == true)
        {
            damageTaken = 30;
            privateTimer = 0.5f;
            StartCoroutine(takeDamage());
            Debug.Log("Ouch");
        }

        if (other.CompareTag("bossRightArm01") && canTakeDamage == true)
        {
            damageTaken = 30;
            privateTimer = 0.5f;
            StartCoroutine(takeDamage());
            Debug.Log("Aie ouille");
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("bossHead01") && canTakeDamage == true)
        {
            damageTaken = 2;
            privateTimer = 0.1f;
            StartCoroutine(takeDamage());
            Debug.Log("Laser Beam");

        }

        if(other.CompareTag("damagedZone") && canTakeDamage == true)
        {
            damageTaken = 10;
            privateTimer = 0.5f;
            StartCoroutine(takeDamage());
            Debug.Log("ça brûle baby boi");
        }
    }

    IEnumerator takeDamage()
    {
        playerHealth -= damageTaken;
        Debug.Log("Player health equal" + playerHealth);
        canTakeDamage = false;
        yield return new WaitForSeconds(privateTimer);
        canTakeDamage = true;

    }

}
