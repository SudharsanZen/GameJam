﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Animator anim;
    bool onLocomotion;
    bool walking;
    bool running;
    bool aiming;
    float horizontal;
    float vertical;
    Vector3 moveDir;

    public float moveSpeed=2;
    public GameObject lookAT;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        getParam();
        handleMovement();
        handleRotation();
        setParam();


       
    }

    void getParam()
    {
        horizontal = Input.GetAxis(InputStatics.horizontal);
        vertical = Input.GetAxis(InputStatics.vertical);
        onLocomotion =(Mathf.Abs(horizontal)>0 ||Mathf.Abs(vertical)>0);
 
        aiming = Input.GetButton(InputStatics.aim);
        /* if (onLocomotion && !Input.GetButton(InputStatics.sprint))
         {
             walking = true;
             running = false;
         }
         else if (onLocomotion)
         {
             walking = false;
             running = true;
         }
         else
         {
             walking = false;
             running = false;
         }*/

        walking = onLocomotion;

    }
    void setParam()
    {
        anim.SetBool(AnimStatics.onLocomotion,onLocomotion);

    }
    void handleMovement()
    {
        //character moves with respect to the world axis
        moveDir =horizontal*Vector3.right+vertical*Vector3.forward;
        transform.position = Vector3.Lerp(transform.position,transform.position+moveDir,Time.deltaTime*moveSpeed);
        
    }


    void handleRotation()
    {
        if (!aiming && moveDir!=Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10);
        }
        else if(aiming)
        {
            aimRot();
        }
       
    }

    void aimRot()
    {
        Plane plane = new Plane(Vector3.up, 0);
        Vector3 worldPosition=transform.position+moveDir;
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (plane.Raycast(ray, out distance))
        {
            
            worldPosition = ray.GetPoint(distance);
            if (((transform.position - worldPosition).magnitude > 0.5f))
                lookAT.transform.position = worldPosition;
            else
            {
                lookAT.transform.position =transform.position+(world-transform.position).normalized*0.5f;
            }
        }
        
        Quaternion lookRot = Quaternion.LookRotation((worldPosition-transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10);
    }
}