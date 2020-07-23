using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTrigger : MonoBehaviour
{
    bool enabledSwitch;
    public GameObject switchDisable;
    public GameObject enemyCommonUpdate;
    public AudioClip chaseMusicStart;
    public AudioClip chaseMusic;
    public audioManager aud;
    public Transform spawnPoint;
    public GameObject boss;
    public float delay=3;
    public float lastTime;
    public bool trigger;
    public bool done;
    public AudioSource xuePio;
    public bool xuePioPlayed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyCommon.player.transform.position - transform.position).magnitude < 4)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                trigger = true;
            }
        }
        if (trigger)
        {
            if (!xuePioPlayed)
            {
                xuePioPlayed = true;
                xuePio.Play();

            }
            if (Time.time > lastTime + delay && !done)
            {
                GameObject a=Instantiate(boss);
                a.transform.position = spawnPoint.transform.position;
                GetChildObject(a.transform,"enemy");
                aud.chaseMusicStart = chaseMusicStart;
                aud.chaseMusic =chaseMusic;
                xuePio.enabled=false;
                audioManager.chaseOn = true;
                done = true;
            }
        }
        else
        {
            lastTime = Time.time;
        }

        if (done)
        {
            if (enemy == null && !enabledSwitch)
            {
                switchDisable.GetComponent<doorSwitch>().enabled = true;
                enabledSwitch = true;
            }
        }
    }
    GameObject enemy;
    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                enemy=parent.GetChild(i).gameObject;
                enemyAi ai = child.gameObject.GetComponent<enemyAi>();
                ai.playerSpotted = true;
                ai.player = enemyCommon.player;
                enemyCommonUpdate.GetComponent<enemyCommon>().aiList.Add(ai);
            }

        }
    }
}
