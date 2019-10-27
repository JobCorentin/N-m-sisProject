using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPatternLoop : MonoBehaviour
{

    protected float bossHealth = 150;
    protected int patternRef;
    protected int patternSaved;
    protected bool canTakeDamage = true;
    protected float seconds;
    protected float weaponDamage = 5;
    public Slider healthBar;
    List<int> patternList = new List<int>();

    public GameObject leftArm01Prefab;
    public Transform leftArmPositionPoint;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            patternList.Add(i);

        }
        patternSaved = -1;
        BossPatternSelection();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = bossHealth;
        seconds++;

        if( patternRef == 0)
        {
            Debug.Log(seconds);
            leftArm01Pattern();
        }
    }

    void BossPatternSelection()
    {
        int iCurrentPattern = Random.Range(0, patternList.Count);
        Debug.Log("PatternList =" + patternList.Count);
        if (iCurrentPattern != -1)
        {
            patternRef = patternList[iCurrentPattern];
            Debug.Log("PatternRef =" + patternRef);
            StartCoroutine(ExecutePattern());
            if (patternSaved != -1)
            {
                patternList.Add(patternSaved);
                Debug.Log("patternRefSaved" + patternSaved + "has been added");
            }
        }
    }

    IEnumerator ExecutePattern()
    {
        if (patternRef == 0)
        {
            Debug.Log("Pattern ref = 0");
            seconds = 0;

        }
        else
        {
            Debug.Log("pattern is 1 or 2");
        }
        yield return new WaitForSeconds(10f);
        RefreshPattern();
    }

    void RefreshPattern()
    {
        patternSaved = patternRef;
        patternList.Remove(patternRef);
        Debug.Log("PatternRef" + patternRef + "has been removed");
        BossPatternSelection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("It has triggered");
        if (collision.CompareTag("Bullet"))
        {
            if (canTakeDamage == true)
            {
                StartCoroutine(takeDamage());
            }
        }
    }

    IEnumerator takeDamage()
    {
        bossHealth -= weaponDamage;
        canTakeDamage = false;
        Debug.Log("bosshealth =" + bossHealth);
        yield return new WaitForSeconds(5f);
        canTakeDamage = true;

    }


    void leftArm01Pattern()
    {
        bool canDo = true;
        if(seconds <= 75f)
        {
            leftArm01Prefab.transform.position = playerPosition.transform.position + new Vector3(0, 2, 0);
            Debug.Log("wait for it");
            
        }
        else if (seconds <= 125f && canDo == true)
        {
            canDo = false;
            leftArm01Prefab.transform.position = playerPosition.transform.position;
            Debug.Log("SecondAccess");
        
        }
        else if (seconds > 125f && canDo == false)
        {
            leftArm01Prefab.transform.position = leftArmPositionPoint.transform.position;
            Debug.Log("ThirdAcces");
        }

    }
}
