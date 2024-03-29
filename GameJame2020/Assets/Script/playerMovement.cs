﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public bool gameComplete=false;
    public bool invincible;
    Animator anim;
    bool onLocomotion;
    bool walking;
    bool running;
    bool aiming;
    float horizontal;
    float vertical;
    public float currHealth;
    public float maxHealth;
    public float currSpine;
    public float maxSpine;
    float currSP;
    float currHP;
    public Image healthBar;
    public Image spineBar;
    Vector3 moveDir;

    public float health=100;
    public float moveSpeed=2;
    public GameObject lookAT;
    public GameObject ragDoll;
    bool ragCreated=false;
    public bool playerDead=false;
    public LayerMask layerMask;

    public bool ShoutItHurts=false;
    public int spineLevel=0;
    int prevSpineLevel;
    public bool shoutSpineHurts;
    AudioSource audioSource;
    public AudioClip[] spineHurt;
    public AudioClip spineDeath;
    public AudioClip[] DeathSounds;
    public AudioClip[] playerHurt;

    GameObject[] playerBody;
    CapsuleCollider playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        audioSource =GetComponent<AudioSource>();
        playerBody=GameObject.FindGameObjectsWithTag("playerBodyMesh");
        currHP = 100;
        currSP = 0;
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
    }
    float last;
    // Update is called once per frame
    void Update()
    {
        if (gameComplete)
        {
            if (last + 1 < Time.time)
            {
              
                GetComponent<Rigidbody>().isKinematic=true;
                enemyCommon.doVictoryDance = true;
                anim.applyRootMotion = true;
                moveSpeed = 0;
                anim.SetBool("gameComplete", true);
                return;
            }
        }
        else
        {
            last=Time.time;
        }
        if (invincible)
        {
            currSpine = 0;
            currHealth = 100;
        }
        makeNoise();
        getParam();
        handleMovement();
        handleRotation();

        manageHealthBars();
        setParam();
    }
    bool playDeadNoise=true;
    void makeNoise()
    {
        if (currSpine >= 100 && playDeadNoise)
        {
            playDeadNoise = false;
            audioSource.clip = spineDeath;
            audioSource.Play();
        }
        else if (currHealth <= 0 && playDeadNoise)
        {
            playDeadNoise = false;
            audioSource.clip = DeathSounds[Random.Range(0, DeathSounds.Length)];
            audioSource.Play();
        }
        else if (ShoutItHurts)
        {
            ShoutItHurts = false;
            audioSource.clip = playerHurt[Random.Range(0, playerHurt.Length)];
            audioSource.Play();
            anim.SetLayerWeight(1, 1);
            anim.Play("hit",1);
        }
        else
        {
            if(anim.GetCurrentAnimatorStateInfo(1).normalizedTime>0.8f)
                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1),0,Time.deltaTime*10));
        }

        if (currSpine >= 75)
        {
            spineLevel = 2;
        }
        else if (currSpine >= 50)
        {
            spineLevel = 1;
        }
        else if (currSpine >= 25)
        {
            spineLevel = 0;
        }


        if (spineLevel > prevSpineLevel)
        {
            prevSpineLevel=spineLevel;
            audioSource.clip =spineHurt[spineLevel];
            audioSource.Play();
        }
        

    }
    void disableEverything()
    {
        /*disable everything when the player dies.....putacase!!!!*/
        //this shit is not optimized but who tf cares doe XDDD
        //feeling cute might just leave this shit here
        for (int i = 0; i < playerBody.Length; i++)
        {
            playerBody[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        playerCollider.enabled = false;
    }
    void manageHealthBars()
    {
        
        currSP =Mathf.LerpUnclamped(currSP,currSpine,Time.deltaTime*3);
        currHP =Mathf.LerpUnclamped(currHP,currHealth,Time.deltaTime*3);

        spineBar.fillAmount =currSP/maxSpine;
        healthBar.fillAmount =currHP/maxHealth;
        if ((currHealth <= 0 || currSP >= maxSpine) && !ragCreated)
        {
            playerDead = true;
            disableEverything();
            ragCreated = true;
            ragDoll=Instantiate(ragDoll);
            ragDoll.transform.position = transform.position;
            
        }

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
        Vector3 worldPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {

            worldPosition = hit.point;
            if (((transform.position - worldPosition).magnitude > 0.5f))
                lookAT.transform.position = worldPosition;
            else
            {
                lookAT.transform.position = transform.position + (worldPosition - transform.position).normalized * 0.5f;
            }
            Vector3 lookDir = (worldPosition - transform.position).normalized;
            lookDir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10);
        }
        else
        {
            lookAT.transform.position = ray.origin+ray.direction*100;
        }

        



    }
}
