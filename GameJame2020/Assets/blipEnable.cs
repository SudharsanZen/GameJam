using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blipEnable : MonoBehaviour
{
    MeshRenderer msh;
    public GameObject blip;
    // Start is called before the first frame update
    void Start()
    {
        msh =blip.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(InputStatics.aim))
            msh.enabled=true;
        if (Input.GetButtonUp(InputStatics.aim))
            msh.enabled = false;
    }
}
