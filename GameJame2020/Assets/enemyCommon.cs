using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCommon : MonoBehaviour
{
    public static GameObject player;
    public static bool playerSpotted;
    public static playerMovement plScript;
    public List<enemyAi> aiList;

    bool set;
    float t1;
    float interval=2;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] enemies =GameObject.FindGameObjectsWithTag("enemy");
        player =GameObject.FindGameObjectWithTag("Player");
        plScript =player.GetComponent<playerMovement>();
        for (int i = 0; i < enemies.Length; i++)
        {
            aiList.Add(enemies[i].GetComponent<enemyAi>());
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        audioManager.chaseOn = playerSpotted;
        if(player==null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (plScript != null)
            plScript.currHealth = Mathf.Clamp(plScript.currHealth, 0, 100);
        int i;
        for ( i = 0; i < aiList.Count; i++)
        {
            if (aiList[i] != null)
            {
                //Debug.Log("spottedd");
                if (aiList[i].playerSpotted)
                {
                    playerSpotted = true;
                    break;
                }
            }
        }

        if (i == aiList.Count)
        {
            //Debug.Log("not spottedd");
            playerSpotted = false;
        }
        /*if (playerSpotted)
        {
            if (!set)
            {
                set = true;
                t1 = Time.time;
            }
        }
        else if(set && t1+interval<Time.time)
        {
            print("reset");
            set = false;
            playerSpotted = false;
        }*/
        /*else
        {
            if(player)
                plScript = player.GetComponent<playerMovement>();
            else
                player = GameObject.FindGameObjectWithTag("Player");
        }*/
    }
}
