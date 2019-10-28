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
    public Transform leftSpawnArmPoint;
    public Transform prepareAttackPoint;
    public Transform leftArmPoint;
    public Transform ImpactPoint;
    public Vector3 pattern01FirstDir;

    // Start is called before the first frame update
    void Start()
    {
        leftArm01 = Instantiate(leftArm01Prefab,leftSpawnArmPoint.transform.position,Quaternion.identity);
        leftArmPoint = leftArm01.transform.GetChild(0).transform;
        ImpactPoint = leftArm01.transform.GetChild(1).transform;

        for (int i = 0; i < 3; i++)
        {
            patternList.Add(i);

        }
        patternSaved = -1;
        BossPatternSelection();
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = bossHealth;
        if(patternRef == 0 && canDo01 == true)
        {
            StartCoroutine(LeftArmPattern01());
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

    IEnumerator LeftArmPattern01()
    {
        Vector3 hit = new Vector3(0, -2, 0);
        float hitSpeed = 5f;
        Debug.Log("Wait for it");
        pattern01FirstDir = prepareAttackPoint.position - leftArmPoint.position;
        leftArm01.transform.Translate(pattern01FirstDir.normalized * leftArm01Speed * Time.deltaTime, Space.World);
        yield return new WaitForSeconds(1.2f);
        canDo01 = false;
        Debug.Log("And it hits");
        ImpactPoint.transform.position = leftArmPoint.transform.position - new Vector3(0,-2,0);
        pattern01FirstDir = ImpactPoint.position - prepareAttackPoint.position;
        leftArm01.transform.Translate(pattern01FirstDir.normalized * Time.deltaTime, Space.World);
        yield return new WaitForSeconds(0.2f);
        Debug.Log("And it's stuck");
        leftArm01.transform.position = leftArmPoint.transform.position;
        yield return new WaitForSeconds(2f);
        Debug.Log("And it goes back");
        leftArm01.transform.position = leftSpawnArmPoint.position;
    }

  
}
