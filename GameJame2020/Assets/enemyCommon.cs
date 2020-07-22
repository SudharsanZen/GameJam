using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCommon : MonoBehaviour
{
    public static GameObject player;
    public static playerMovement plScript;
    // Start is called before the first frame update
    void Awake()
    {
        
        player =GameObject.FindGameObjectWithTag("Player");
        plScript =player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (plScript != null)
            plScript.currHealth = Mathf.Clamp(plScript.currHealth, 0, 100);
        /*else
        {
            if(player)
                plScript = player.GetComponent<playerMovement>();
            else
                player = GameObject.FindGameObjectWithTag("Player");
        }*/
    }
}
