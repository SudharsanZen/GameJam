using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightOnOff : MonoBehaviour
{
    public bool bulbOn;
    public GameObject on;
    public GameObject off;
    AudioSource audioS;
    private void Start()
    {
        audioS =gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (bulbOn)
        {
           
            on.SetActive(true);
            off.SetActive(false);
        }
        else
        {

            on.SetActive(false);
            off.SetActive(true);
        }
        audioS.mute = !bulbOn;
    }
}
