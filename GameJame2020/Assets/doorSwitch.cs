using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSwitch : MonoBehaviour
{
    Transform player;
    public GameObject pivot;
    public bool on;
    public LayerMask lmask;
    public float ZAngleOpen=-90;
    public float ZAngleClosed;
    public bool dependsOnPower=false;
    public GameObject powerSwitch;
    lightSwitch power;
    // Start is called before the first frame update
    void Start()
    {
        if (dependsOnPower)
        {
            power=powerSwitch.GetComponent<lightSwitch>();
        }
        player=enemyCommon.player.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (dependsOnPower)
        {
            if (!power.on && on)
                on = false;
                
        }
        Vector3 angles = pivot.transform.localRotation.eulerAngles;
        if (on)
        {
            
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, Quaternion.Euler(angles.x, ZAngleOpen, angles.z), Time.deltaTime * 10);
        }
        else
        {
            pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, Quaternion.Euler(angles.x, ZAngleClosed, angles.z), Time.deltaTime * 10);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        //print("near");
        if (Input.GetKeyDown(KeyCode.E) && other.tag.Equals("Player"))
        {
            // print("on/off");

            on = !on;
        }
    }
}
