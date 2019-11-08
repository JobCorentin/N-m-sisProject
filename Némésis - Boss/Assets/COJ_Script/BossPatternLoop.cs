using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPatternLoop : MonoBehaviour
{

    protected float bossHealth = 300;
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

    public GameObject rightArm01;
    public GameObject rightArm01Prefab;
    public Transform rightArmSpawnPoint;
    protected bool canDoRightArm01 = true;
    protected Animator rightArm01Animator;
    public GameObject damagedGround;
    // Start is called before the first frame update
    void Start()
    {
        leftArm01 = Instantiate(leftArm01Prefab,leftSpawnArmPoint.transform.position,Quaternion.identity);
        leftArmPoint = leftArm01.transform.GetChild(0).transform;
        leftArm01Rb = leftArm01.GetComponent<Rigidbody2D>();

        Head01 = Instantiate(Head01Prefab, HeadSpawnPoint.transform.position, Quaternion.identity);
        HeadPoint = Head01.transform.GetChild(0).transform;

        rightArm01 = Instantiate(rightArm01Prefab, rightArmSpawnPoint.transform.position, Quaternion.identity);
        rightArm01Animator = rightArm01.GetComponent<Animator>();

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
            
        }
        if (patternRef == 2 && canDoRightArm01 == true)
        {
            StartCoroutine(RightArm01Pattern());
            canDoRightArm01 = false;
           
        }




    }
    private void Update()
    {
        bossDeath();
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
           
            pattern01FirstDir = leftSpawnArmPoint.position - leftArmPoint.position;
            leftArm01Rb.velocity = pattern01FirstDir * returnSpeed;
            pattern1Timer -= Time.deltaTime;
            yield return null;
        }
        
        pattern01FirstDir = new Vector3(0, 0, 0);
        leftArm01Rb.velocity = pattern01FirstDir * 0;
        yield return new WaitForSeconds(1f);
       
        leftArm01Rb.position = leftSpawnArmPoint.transform.position;
        yield return new WaitForSeconds(2f);
        Debug.Log("Refrest the pattern");
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
        yield return new WaitForSeconds(1f);
        Debug.Log("Refrest the pattern");
        RefreshPattern();
    }

    IEnumerator RightArm01Pattern()
    {
        rightArm01Animator.SetBool("doPattern", true);
        Debug.Log("Access 1");
        yield return new WaitForSeconds(1f);
        Debug.Log("Access 2");
        rightArm01Animator.SetBool("doPattern", false);
        yield return new WaitForSeconds(1f);
        damagedGround.SetActive(true);
        Debug.Log("Acces 3");
        Debug.Log("Refrest the pattern");
        canDoRightArm01 = true;
        RefreshPattern();
        yield return new WaitForSeconds(2f);
        damagedGround.SetActive(false);
    }
   
    void bossDeath()
    {
        if (bossHealth <= 0)
        {
            Debug.Log(" you won !");
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
