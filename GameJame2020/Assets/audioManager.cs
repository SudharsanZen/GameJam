using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioClip chaseMusic;
    AudioSource aSource;
    public bool prevStateChaseOn;
    public static bool chaseOn;
    public float chaseTransTime;
    float lastSeenTime;
    // Start is called before the first frame update
    void Start()
    {
        aSource =GetComponent<AudioSource>();
        chaseOn = false;
        prevStateChaseOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (chaseOn)
        {
            lastSeenTime =Time.time;
            prevStateChaseOn = true;
            
        }
        else
        {
            if (chaseTransTime + lastSeenTime < Time.time)
            {
                chaseOn = false;
                prevStateChaseOn = false;
            }
        }

        if (prevStateChaseOn)
        {
            if (aSource.volume<1)
            {
                aSource.clip = chaseMusic;
                aSource.Play();
                aSource.volume = 1;
               
                
            }

        }
        else
        {
            aSource.volume = Mathf.LerpUnclamped(aSource.volume, 0, Time.deltaTime);
        }
    }
}
