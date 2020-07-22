using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insectTrigger : MonoBehaviour
{
    public bool shouldPressE = false;
    public string insectGroupName="insects1";
    bool triggeredOnce;
    GameObject[] insects;
    // Start is called before the first frame update
    void Start()
    {
        insects = GameObject.FindGameObjectsWithTag(insectGroupName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!triggeredOnce)
        {
            if (shouldPressE)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    activateInsetcs();
                }
            }
            else
            {
                activateInsetcs();
            }
        }
    }

    void activateInsetcs()
    {
        triggeredOnce = true;
        for (int i = 0; i < insects.Length; i++)
        {
            insects[i].GetComponent<insects>().followPlayer = true;
        }
    }

}
