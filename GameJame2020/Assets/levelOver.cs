using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelOver : MonoBehaviour
{
    AudioSource aSource;
    bool startVictory=false;
    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!startVictory)
            {
                aSource.Play();
                startVictory = true;
            }
            enemyCommon.plScript.gameComplete = true;
        }
    }
}
