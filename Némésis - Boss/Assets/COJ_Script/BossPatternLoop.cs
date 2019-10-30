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
    protected float weaponDamage = 5;
    public Slider healthBar;
    List<int> patternList = new List<int>();

    protected GameObject leftArm01;


    // leftArmPattern condition and object
    public GameObject leftArm01Prefab;
    protected float leftArm01Speed = 5f;
    protected bool canDo01 = true;
    protected bool canDo02 = false;
    protected bool canDo03 = false;
    protected bool canDo04 = false;
    public Transform leftSpawnArmPoint;
    public Transform prepareAttackPoint;
    public Transform leftArmPoint;
    public Transform ImpactPoint;
    public Vector3 pattern01FirstDir;
    public Rigidbody2D leftArm01Rb;
    public GameObject impactPointSpawnPrefab;
    public Color dashNowColor = Color.red;
    public Color normalColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        leftArm01 = Instantiate(leftArm01Prefab,leftSpawnArmPoint.transform.position,Quaternion.identity);
        leftArmPoint = leftArm01.transform.GetChild(0).transform;
        leftArm01Rb = leftArm01.GetComponent<Rigidbody2D>();

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


        // Pattern LeftArm01
        if(patternRef == 0 && canDo01 == true)
        {
            StartCoroutine(LeftArmPattern01Part1());
            canDo01 = false;
        }
     
        // Pattern Left Arm01




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
            canDo01 = true;
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
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;

    }

    IEnumerator LeftArmPattern01Part1()
    {
        float moveSpeed = 5f;
        float pattern1Timer = 2f;

        while(pattern1Timer > 0)
        {
            Debug.Log("Wait for it");
            pattern01FirstDir = prepareAttackPoint.position - leftArmPoint.position;
            leftArm01Rb.velocity = pattern01FirstDir * moveSpeed;
            pattern1Timer -= Time.deltaTime;
            if (pattern1Timer <= 0.3f && pattern1Timer > 0.1f)
            {
                leftArm01.GetComponent<SpriteRenderer>().material.color = dashNowColor;
            }
            else
            {
                leftArm01.GetComponent<SpriteRenderer>().material.color = normalColor;
            }
            yield return null;
        }

        StartCoroutine(LeftArmPattern01Part2());
      
    }

    IEnumerator LeftArmPattern01Part2()
    {
        
        {
            
            float impactSpeed = 15f;
            Debug.Log("And it hits");
            GameObject impactPointSpawn = Instantiate(impactPointSpawnPrefab, ImpactPoint.transform.position, Quaternion.identity);
            pattern01FirstDir = impactPointSpawn.transform.position - leftArmPoint.position;
            leftArm01Rb.velocity = pattern01FirstDir * impactSpeed;
            yield return new WaitForSeconds(0.1f);
            Destroy(impactPointSpawn);
            StartCoroutine(LeftArmPattern01Part3());
        }
       
      
    }

    IEnumerator LeftArmPattern01Part3()
    {
        Debug.Log("and it's stuck");
        pattern01FirstDir = new Vector3(0, 0, 0);
        leftArm01Rb.velocity = pattern01FirstDir * 1;
        yield return new WaitForSeconds(2f);
        StartCoroutine(LeftArmPattern01Part4());
    }

    IEnumerator LeftArmPattern01Part4()
    {
        float pattern1Timer = 1f;
        float returnSpeed = 10f;
        while (pattern1Timer > 0)
        {
            Debug.Log("And it's comes back");
            pattern01FirstDir = leftSpawnArmPoint.position - leftArmPoint.position;
            leftArm01Rb.velocity = pattern01FirstDir * returnSpeed;
            pattern1Timer -= Time.deltaTime;
            yield return null;
        }
        
        pattern01FirstDir = new Vector3(0, 0, 0);
        leftArm01Rb.velocity = pattern01FirstDir * 0;
        yield return new WaitForSeconds(1f);
        Debug.Log("Back to mama");
        leftArm01Rb.position = leftSpawnArmPoint.transform.position;
        
    }
  
}
