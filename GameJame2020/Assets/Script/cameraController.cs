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

    // Update is called once per frame
    void Update()
    {
        if (plScript.currHealth > 0)
        {

            transform.position = Vector3.Lerp(transform.position, followTarget.position + initOffset, Time.deltaTime * camFollowSpeed);
            lastPos = transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, lastPos+transform.forward*1, Time.deltaTime * camFollowSpeed);
        }

    }
}
