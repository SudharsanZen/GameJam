using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSwitch : MonoBehaviour
{
    public bool on=false;
    public string lightTag="lightSwitch1";
    public Transform moveablePart;
    public Transform player;
    public LayerMask lmask;
    public GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        player =GameObject.FindGameObjectWithTag("Player").transform;
        lights = GameObject.FindGameObjectsWithTag(lightTag);
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
        if ((player.position - transform.position).magnitude < 2f)
        {
            Debug.DrawLine(transform.position, player.position);
           

            if (Physics.Linecast(transform.position, player.position, lmask))
            {
                //print("near");
                if (Input.GetKeyDown(KeyCode.E))
                {
                   // print("on/off");

                    on =!on;
                    GetComponent<AudioSource>().Play();


                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].GetComponent<lightOnOff>().bulbOn = on;
                    }

                   
                    
                }
            }

            if (on)
            {
                moveablePart.transform.localRotation = Quaternion.Lerp(moveablePart.transform.localRotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * 10);
            }
            else
            {
                moveablePart.transform.localRotation = Quaternion.Lerp(moveablePart.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
            }
        }


    }
}
