using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioClip normMusic;
    public AudioClip chaseMusicStart;
    bool chaseStartMusicEnded;
    public AudioClip chaseMusic;
    AudioSource aSource;
    public bool prevStateChaseOn;
    public static bool chaseOn;
    public float chaseTransTime;
    float lastSeenTime;
    bool switchedClip;
    // Start is called before the first frame update
    void Start()
    {
        aSource =GetComponent<AudioSource>();
        chaseOn = false;
        prevStateChaseOn = false;
        aSource.clip =normMusic;
        aSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (chaseOn)
        {
            switchedClip = false;
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
            if (!aSource.clip.Equals(chaseMusic) && chaseStartMusicEnded)
            {
                aSource.clip = chaseMusic;
                aSource.Play();
                aSource.loop = true;
                aSource.volume = 1;
            }
            if(!aSource.clip.Equals(chaseMusicStart) && !chaseStartMusicEnded &&chaseOn) 
            {
                aSource.loop = false;
                aSource.clip = chaseMusicStart;
                aSource.Play();
                aSource.volume = 1;
            }
            if (aSource.clip.Equals(chaseMusicStart) && !chaseStartMusicEnded)
            {
              
                if (aSource.time/ aSource.clip.length > 0.9999f)
                {
                    chaseStartMusicEnded = true;
                }
            }

        }
        else
        {
            
            if(!switchedClip)
                aSource.volume = Mathf.LerpUnclamped(aSource.volume, 0, Time.deltaTime);
            if (aSource.volume < 0.01f && !chaseOn)
            {
                aSource.loop = true;
                chaseStartMusicEnded = false;
                switchedClip = true;
                prevStateChaseOn = false;
                aSource.clip =normMusic;
                aSource.volume = 1;
                aSource.Play();
            }
        }

        
    }
}
