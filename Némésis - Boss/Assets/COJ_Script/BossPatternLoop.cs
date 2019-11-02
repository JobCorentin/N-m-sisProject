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
    protected bool canDoAnotherOne = true;
    protected float weaponDamage = 5;
    public Slider healthBar;
    List<int> patternList = new List<int>();





    // leftArm01Pattern condition and object
    protected GameObject leftArm01;
    public GameObject leftArm01Prefab;
    protected float leftArm01Speed = 5f;
    protected bool canDoLeftArm01 = true;
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

    //Head01Pattern condition and object
    protected GameObject Head01;
    public GameObject Head01Prefab;
    public LineRenderer laserBeamRenderer;
    public GameObject newBeamImpactPoint;
    protected bool canDoHead01 = true;
    public Transform HeadPoint;
    public GameObject newBeamPoint;
    public Transform BeamPoint;
    public Transform HeadSpawnPoint;
    private LineRenderer Beam;

    //RightArm01Pattern Condition and object
    private static Transform[] goToPoints;
    public GameObject rightArm01;
    public GameObject rightArm01Prefab;
    protected Rigidbody2D rightArm01Rb;
    public Transform rightArmSpawnPoint;
    private Transform target;
    private int wavePointsIndex = 0;
    protected bool canDoRightArm01 = true;


    // Start is called before the first frame update
    void Start()
    {
        leftArm01 = Instantiate(leftArm01Prefab,leftSpawnArmPoint.transform.position,Quaternion.identity);
        leftArmPoint = leftArm01.transform.GetChild(0).transform;
        leftArm01Rb = leftArm01.GetComponent<Rigidbody2D>();

        Head01 = Instantiate(Head01Prefab, HeadSpawnPoint.transform.position, Quaternion.identity);
        HeadPoint = Head01.transform.GetChild(0).transform;

        rightArm01 = Instantiate(rightArm01Prefab, rightArmSpawnPoint.transform.position, Quaternion.identity);
        rightArm01Rb = rightArm01.GetComponent<Rigidbody2D>();

        for (int i = 0; i < 3; i++)
        {
            patternList.Add(i);

        }
        patternSaved = -1;

        goToPoints = new Transform[transform.childCount];
        for (int j = 0; j < goToPoints.Length; j++)
        {
            goToPoints[j] = rightArm01.transform.GetChild(j);
        }
        BossPatternSelection();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = bossHealth;


        // Pattern LeftArm01
        if(patternRef == 0 && canDoLeftArm01 == true)
        {
            StartCoroutine(LeftArmPattern01Part1());
            canDoLeftArm01 = false;
        }

        //Pattern Head01
        if (patternRef == 1 && canDoHead01 == true)
        {
            StartCoroutine(Head01Pattern());
            canDoHead01 = false;
            Debug.Log("Beam activated");
        }
        if (patternRef == 2 && canDoRightArm01 == true)
        {
            StartCoroutine(RightArm01PAttern());
            canDoRightArm01 = false;
            Debug.Log("SLASH !");
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
            if (patternSaved != -1)
            {
                patternList.Add(patternSaved);
                Debug.Log("patternRefSaved" + patternSaved + "has been added");
            }
        }
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
        yield return new WaitForSeconds(2f);
        RefreshPattern();
        canDoLeftArm01 = true;
    }
  

    IEnumerator Head01Pattern()
    {
        bool canSpawn = true;
        float beamPatternTimer = 4f;

        if (canSpawn == true)
        {
            Beam = Instantiate(laserBeamRenderer, HeadPoint.transform.position, Quaternion.identity);
            newBeamPoint = Instantiate(newBeamImpactPoint, BeamPoint.transform.position, Quaternion.identity);
            canSpawn = false;
        }

        while (beamPatternTimer > 0)
        {
            Beam.SetPosition(0, HeadPoint.position);
            Beam.SetPosition(1, newBeamPoint.transform.position);
            beamPatternTimer -= Time.deltaTime;
       
            yield return null;
        }
        if (beamPatternTimer < 0)
        {
            Destroy(Beam);
            Destroy(newBeamPoint);
            canDoHead01 = true;
            
        }
        yield return new WaitForSeconds(4f);
        RefreshPattern();
    }

    IEnumerator RightArm01PAttern()
    {
        float attackSpeed = 15f;
        float patternTimer = 1.2f;
        bool trigger01 = true;
        bool trigger02 = false;
        bool trigger03 = false;
        bool trigger04 = false;
        bool trigger05 = false;
        target = goToPoints[0];
        yield return new WaitForSeconds(0.5f);
        while (patternTimer > 0)
        {
            Vector3 attackDir = target.position - rightArm01.transform.position;
            rightArm01Rb.velocity = attackDir * attackSpeed;
            if(patternTimer >= 1.15f && trigger01 == true)
            {
                GetNextPoints();
                trigger01 = false;
                trigger02 = true;
            }

            if(patternTimer >= 1f && trigger02 == true)
            {
                GetNextPoints();
                trigger02 = false;
                trigger03 = true;
            }

            if (patternTimer >= 0.8f && trigger03 == true)
            {
                GetNextPoints();
                trigger03 = false;
                trigger04 = true;
            }

            if (patternTimer >= 0.5f && trigger04 == true)
            {
                GetNextPoints();
                trigger04 = false;
                trigger05 = true;
            }

            if (patternTimer >= 0.3f && trigger05 == true)
            {
                GetNextPoints();
                trigger05 = false;
                
            }
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        RefreshPattern();

    }

    void GetNextPoints()
    {
        wavePointsIndex++;
        target = goToPoints[wavePointsIndex];
    }
}
