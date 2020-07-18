using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    Transform followTarget;
    Vector3 initOffset;
    public float camFollowSpeed=8;
    // Start is called before the first frame update
    void Start()
    {
        followTarget = player.transform;
        initOffset =transform.position-followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =Vector3.Lerp(transform.position,followTarget.position+initOffset,Time.deltaTime*camFollowSpeed);
    }
}
