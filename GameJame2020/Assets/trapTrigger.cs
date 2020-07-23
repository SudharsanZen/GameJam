using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapTrigger : MonoBehaviour
{
    bool spawnedEnemy;
    public GameObject enemy;
    public string spawnPointName="spawn1";
    GameObject[] spawnPoints;
    public bool trapsOn;
    public int numOfTrapTrrigerAtATime;
    public GameObject[] traps;
    bool trapsTriggered;
    public string trapGroup="trapGroup1";
    int currIndex;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints =GameObject.FindGameObjectsWithTag(spawnPointName);
        currIndex = 0;
        traps =GameObject.FindGameObjectsWithTag(trapGroup);
        if (numOfTrapTrrigerAtATime > traps.Length)
            numOfTrapTrrigerAtATime = traps.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyCommon.player.transform.position - transform.position).magnitude < 3)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                trapsOn = true;
            }
        }
        if (trapsOn)
        {
            if (!spawnedEnemy)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    GameObject e=Instantiate(enemy);
                    e.transform.position =spawnPoints[i].transform.position;
                    GetChildObject(e.transform,"enemy");
                   
                }
                spawnedEnemy = true;
            }
            trapsTrig();
        }
       
    }
    void trapsTrig()
    {
        if (!trapsTriggered)
        {
            activateSelectedTraps();
            trapsTriggered = true;
        }
        else if (!traps[currIndex].GetComponent<trap>().TrapTrigger)
        {
            currIndex+=numOfTrapTrrigerAtATime;
            trapsTriggered = false;
        }
        if (currIndex >= traps.Length)
            currIndex = 0;
    }

    void activateSelectedTraps()
    {
        for (int i = currIndex; i < currIndex+numOfTrapTrrigerAtATime; i++)
        {
            trap t = traps[i % traps.Length].GetComponent<trap>();
            if (!t.TrapTrigger)
                t.TrapTrigger = true;

        }
    }

    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                enemyAi ai=child.gameObject.GetComponent<enemyAi>();
                ai.playerSpotted= true;
                ai.player = enemyCommon.player;
            }
           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                trapsOn = true;
            }
        }
    }
}
