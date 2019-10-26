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
    void Update()
    {
        healthBar.value = bossHealth;
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
        yield return new WaitForSeconds(3f);
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
}
