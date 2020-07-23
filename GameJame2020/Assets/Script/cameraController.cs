using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    Vector3 lastPos;
    public GameObject player;
    playerMovement plScript;
    Transform followTarget;
    Vector3 initOffset;
    public float camFollowSpeed=8;
    // Start is called before the first frame update
    void Start()
    {
        plScript =player.GetComponent<playerMovement>();
        followTarget = player.transform;
        initOffset =transform.position-followTarget.position;
    }
    float ang = 5;
    // Update is called once per frame
    void Update()
    {
        if (plScript.currHealth > 0)
        {

            transform.position = Vector3.Lerp(transform.position, followTarget.position + initOffset, Time.deltaTime * camFollowSpeed);
            lastPos = transform.position;
        }
        if(plScript.currHealth < 0 || enemyCommon.doVictoryDance)
        {

            if (enemyCommon.doVictoryDance)
            {
                ang++;
                if (ang > 360)
                    ang = 0;
                Quaternion rot = Quaternion.Euler(0, ang, 0);
                Vector3 newPos = rot * (player.transform.forward) * 2;
                transform.position = Vector3.Lerp(transform.position, followTarget.position + newPos, Time.deltaTime * 10);
                transform.LookAt(followTarget.position);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, lastPos + transform.forward * 1, Time.deltaTime * camFollowSpeed);
            }
        }

    }
}
