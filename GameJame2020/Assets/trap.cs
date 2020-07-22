using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    public Vector3 rayOriginOffset;
    public Vector3 rayDistancing;
    Vector3 originalPose;
    Vector3 originalPoseBase;
    public Transform horns;
    public LayerMask lMask;
    public float height=2;
    public bool TrapTrigger;
    public float hornsStayTime=1;
    bool timeSet;
    float lastShot;
    float lastTime;
    bool playerDamagedOnce=false;
    public float delay =1;
    int count=0;
    // Start is called before the first frame update
    void Start()
    {
        originalPoseBase =transform.position;
        originalPose =horns.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
     
        if (TrapTrigger && Time.time > lastTime + delay)
        {
            count++;
            if (count == 3)
            {
                hitRays();
            }
            if (!timeSet)
            {
                playerDamagedOnce = true;
                timeSet = true;
                lastShot = Time.time;
            }
            horns.transform.localPosition = Vector3.Lerp(horns.transform.localPosition, originalPose + horns.transform.forward * height, Time.deltaTime * 20);
        }
        else if (!TrapTrigger)
        {
            count = 0;
            timeSet = false;
            lastTime = Time.time;
            horns.transform.localPosition = Vector3.Lerp(horns.transform.localPosition, originalPose, Time.deltaTime * 10);
            transform.position = Vector3.Lerp(transform.position, originalPoseBase, Time.deltaTime * 20);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPoseBase + horns.transform.forward * 0.1f, Time.deltaTime * 20);
        }

        if (lastShot + 1 < Time.time && TrapTrigger &&timeSet)
            TrapTrigger = false;
    }


    void hitRays()
    {
        //List<enemyAi> enm =;
        Vector3 origin=horns.transform.position+rayOriginOffset;
        
        origin.y =transform.position.y;
        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                Vector3 newOrigin =origin;
                newOrigin.z =origin.z+rayDistancing.z*i;
                newOrigin.x =origin.x+rayDistancing.x*k;
                Ray r = new Ray(newOrigin, horns.transform.forward * 2);
                Debug.DrawRay(r.origin,r.direction*2);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 2, lMask))
                {
                    if (hit.collider.gameObject.tag.Equals("enemy"))
                    {
                        print("enemy hit!");
                        hit.collider.gameObject.GetComponent<enemyAi>().currHealth -= 25;
                    }
                    if(hit.collider.gameObject.tag.Equals("Player"))
                    {
                        enemyCommon.plScript.ShoutItHurts = true;
                        playerDamagedOnce = false;
                        enemyCommon.plScript.currHealth -= 50;
                    }

                }
            }
        }
    }
}
