﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyAi : MonoBehaviour
{
    public float playerDamage = 10;
    public AudioSource deathNoise;
    public AudioSource slashNoise;
    public AudioSource footSteps;
    public AudioSource lowHealthGrunt;
    public AudioSource monsterHurt;
    public float footOffset = 0.1f;
    public Transform rightLegHint;
    public Transform leftLegHint;
    public bool trapHit;
    public GameObject ragdoll;
    public List<Transform> paths;
    public LayerMask layerMask;
    public GameObject healthBar;
    
    public float runSpeed=1;
    public float walkSpeed=2;
    float timeSinceLastSeen=0;
    float coolDownInterval=4;
    public float enemyFieldOfView=80;
    public float enemyViewRange=10;
    public float attackRange=1f;
    public float stopRange=0.5f;
    float prevHealth;
    public float currHealth=100;
    public float maxHealth=100;

    public GameObject player;
    playerMovement plScript;
    NavMeshAgent nMesh;
   



    Animator anim;
    public bool playerSpotted=false;
    bool inSight=false;
    bool attack=false;
    bool playerDead=false;
    bool dead=false;
    bool scout=true;
    bool attackOver=false;
    bool stopRunning=true;
    int currPath = 0;
    float nextScoutStartTime;
    float initRunspeed;
    float idleTimeAfterWalk = 1;
    bool cameNearPath = false;

    bool hitOnce = false;
    Vector3 lastDestination;
    Image healthImage;
    // Start is called before the first frame update
    void Start()
    {
       
        healthImage =healthBar.GetComponent<Image>();
        initRunspeed = runSpeed;
        currHealth = maxHealth;
        prevHealth = maxHealth;
        plScript =player.GetComponent<playerMovement>();
        nMesh =GetComponent<NavMeshAgent>();
        nMesh.SetDestination(paths[0].position);
        nMesh.speed =walkSpeed;
        //player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }
    bool setAudio = false;
    // Update is called once per frame
    void Update()
    {
        
        
        /*if (playerSpotted)
        {
            enemyCommon.playerSpotted = true;
            if (setAudio && !audioManager.chaseOn)
                audioManager.chaseOn = true;
                
            setAudio = true;
            audioManager.chaseOn = playerSpotted;
        }
        else if(setAudio)
        {
            audioManager.chaseOn = playerSpotted;
        }*/

        healthImage.fillAmount =currHealth/maxHealth;
        healthBar.transform.LookAt(Camera.main.transform.position);
        if (!dead)
        {
           
            if (anim != null)
                attackOver = anim.GetBool(minionStatics.attackOver);
            else
                anim = GetComponent<Animator>();
            sight();
            setParam();
            doEnemyActions();
            nMesh.stoppingDistance = stopRange;
        }
        else
        {

           
           ragdoll =Instantiate(ragdoll);
           ragdoll.transform.position = transform.position;
           ragdoll.transform.rotation = transform.rotation;
            for (int i = 0; i < ragdoll.transform.childCount; i++)
            {
                Transform org =transform.GetChild(i);
                Transform rg =ragdoll.transform.GetChild(i);
                if (org != null && rg != null)
                {
                    rg.transform.localPosition = org.transform.localPosition;
                    rg.transform.localRotation = org.transform.localRotation;
                }
            }
            deathNoise.Play();
            Destroy(gameObject);
        }
       
    }
    float lastChange=0;
    bool lowHealthplay=true;
    float slowDownTime=0.2f;
    bool playHurt=false;
    float nextLowPlay = 50;
    void doEnemyActions()
    {
        if (currHealth < nextLowPlay && lowHealthplay)
        {
            lowHealthplay = false;
            print("low health");
            lowHealthGrunt.Play();
        }
        else if (!lowHealthplay && currHealth > nextLowPlay)
        {
            lowHealthplay = true;
        }
        if (prevHealth > currHealth )
        {
            prevHealth = currHealth;
            anim.SetLayerWeight(2, 1);
            anim.Play("hit", 2);
            lastChange = Time.time;
            if (playHurt)
            {
                playHurt = false;
                monsterHurt.Play();
            }
        }
        else
        {
            playHurt = true;
            if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.8f)
                anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 0, Time.deltaTime * 10));
        }
        if (lastChange + slowDownTime > Time.time)
        {
            runSpeed = initRunspeed*0.8f;
        }
        else
        {
            runSpeed = initRunspeed;
        }
        if (playerSpotted && !plScript.playerDead)
        {
            //print((player.transform.position - transform.position).magnitude);
            if ((player.transform.position - transform.position).magnitude < attackRange)
            {
                attack = true;
                attackPlayer();
                
               
            }
            else //if (attackOver)
            {

                followPlayer();
            }
            

        }
        else
        {

            followPath();
        }
        if (currHealth <= 0)
        {
            dead = true;
        }
    }
    void followPlayer()
    {
       if(attackOver)
        attack = false;
        nMesh.speed = runSpeed;
        stopRunning = false;
        lastDestination = player.transform.position;
        lastDestination.y =transform.position.y;
        if(lastDestination != nMesh.destination)
            nMesh.SetDestination(lastDestination);
       
    }
    bool playSlashNoise = false;
    void attackPlayer()
    {
        Vector3 dist = player.transform.position - transform.position;

        if (dist.magnitude < attackRange)
        {

            anim.Play("attakc_Minion", 1);


            if (anim.GetFloat(minionStatics.attack1Weight) > 0.9f && !hitOnce)
            {
               // print("hit");
                hitOnce = true;
                plScript.currHealth -= playerDamage;
                plScript.ShoutItHurts = true;
                /*if (Mathf.Abs(angleBetweenPlayer) < enemyFieldOfView)
                {
                    if (!Physics.Linecast(transform.position + new Vector3(0, 0.5f, 0), player.transform.position + new Vector3(0, 0.5f, 0), layerMask) && dist.magnitude < attackRange)
                    {

                    }
                }*/
            }


        }
        //if (anim.GetFloat("hitReset") > 0.9f)
        // hitOnce = false;
        AnimatorStateInfo aStateInfo = anim.GetCurrentAnimatorStateInfo(1);
        
        if (aStateInfo.IsName("attakc_Minion"))
        {
            if (aStateInfo.normalizedTime < 0.1f && !playSlashNoise)
            {
                playSlashNoise = true;
            }
            if (aStateInfo.normalizedTime > 0.9f)
            {
                playSlashNoise = false;
                hitOnce = false;
            }

            if (playSlashNoise)
            {
                slashNoise.Play();
                playSlashNoise = false;
            }
        }


        if ((player.transform.position - transform.position).magnitude < stopRange)
        {
            nMesh.velocity = Vector3.zero;
            nMesh.speed = 0;

            stopRunning = true;
        }
        else
        {
            
         
            stopRunning = false;
            nMesh.speed = runSpeed;
        }
        Vector3 lookDir = (player.transform.position - transform.position);
        lookDir.y = 0;
        lookDir = lookDir.normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(lookDir,Vector3.up),Time.deltaTime*4);

    }
    void setParam()
    {

        if(attack)
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1, Time.deltaTime * 20));//set attack layer weight to max
        else
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0, Time.deltaTime * 20));//set attack layer weight to zero
        anim.SetBool(minionStatics.playerSpotted,playerSpotted);
        anim.SetBool(minionStatics.playerDead,playerDead);
        anim.SetBool(minionStatics.attack,attack);
        anim.SetBool(minionStatics.scout,scout);
        anim.SetBool(minionStatics.dead,dead);
        anim.SetBool(minionStatics.playerDead,playerDead);
        anim.SetBool(minionStatics.playerDead,playerDead);
        anim.SetBool(minionStatics.stopRunning,stopRunning);
    }

   
    void followPath()
    {
        //print("poop");
        //make the enemy follow the path in the list of transforms
        //enemy will stop at each path for the specified anount of time in "idleTimmeAfterWalk"
        Vector3 temp= paths[currPath].position;
        temp.y =transform.position.y;
        if (( temp- transform.position).magnitude < 0.2f && scout)
        {
            cameNearPath = true;
            nextScoutStartTime =Time.time+idleTimeAfterWalk;
            scout = false;
        }
        if (!scout && !playerSpotted && cameNearPath)
        {
            if (Time.time > nextScoutStartTime)
            {
                currPath++;
                currPath =currPath%paths.Count;
                temp = paths[currPath].position;
                temp.y = transform.position.y;
                nMesh.SetDestination(temp);
                scout = true;
                cameNearPath = false;
            }
        }
        attack = false;
    }
    float angleBetweenPlayer;
    void sight()
    {
        //checks if there is any obstacles between the player and the mosnter
        //and also checks if the player is in the field of view of the monster and within the range
        Vector3 offset =new Vector3(0,1,0);
        //Debug.DrawLine(transform.position+offset,player.transform.position+offset,Color.green);

        //Debug.DrawRay(transform.position + offset, (Quaternion.Euler(0,enemyFieldOfView,0)* transform.forward)*enemyViewRange , Color.blue);
        //Debug.DrawRay(transform.position + offset, (Quaternion.Euler(0,-enemyFieldOfView,0)* transform.forward)*enemyViewRange , Color.blue);

        angleBetweenPlayer =Vector3.SignedAngle(transform.forward,(player.transform.position-transform.position).normalized,Vector3.up);
        //print("Angle: "+angleBetweenPlayer);
        if (!Physics.Linecast(transform.position + offset, player.transform.position+offset, layerMask) && (Mathf.Abs(angleBetweenPlayer)<enemyFieldOfView||Input.GetButton(InputStatics.fire)))
        {
            
            lastDestination = player.transform.position;
            timeSinceLastSeen = Time.time;
            playerSpotted = true;
        }
        else
        {
            
            if (timeSinceLastSeen + coolDownInterval < Time.time )
            {
                if (playerSpotted)
                {
                   
                    nMesh.destination = paths[currPath].position;
                }
                if(Physics.Linecast(transform.position + offset, player.transform.position + offset, layerMask))
                    playerSpotted = false;
                else
                    lastDestination = player.transform.position;

            }
        }
    }

    void ikHandling()
    {
        Vector3 dist = player.transform.position - transform.position;

        if (dist.magnitude < attackRange)
        {

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, anim.GetFloat(minionStatics.attack1Weight));


            anim.SetIKPosition(AvatarIKGoal.LeftHand, player.transform.position + new Vector3(0, 0.5f, 0));

        }
       
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (attack)
            ikHandling();
        FootIk();
    }
    
    bool playL;
    bool playR;
    void FootIk()
    {
        float legRayDist = 2;
        float rweight = anim.GetFloat("rightFootWeight");
        float lweight = anim.GetFloat("leftFootWeight");

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lweight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, lweight);

        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rweight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rweight);
        RaycastHit hit;
        Ray Lray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        Debug.DrawRay(Lray.origin, Lray.direction * legRayDist);
        if (Physics.Raycast(Lray, out hit, legRayDist, layerMask))
        {
            if (!playL && lweight > 0.9f)
            {
                playL = true;
                footSteps.Play();
            }
            Vector3 footPose = hit.point;
            footPose.y += footOffset;
            //print(hit.distance);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPose);
            Vector3 forward = Vector3.Cross(anim.GetBoneTransform(HumanBodyBones.LeftFoot).right, hit.normal);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(-forward, hit.normal));

        }
        if (lweight < 0.5f)
            playL = false;

        Ray Rray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        Debug.DrawRay(Rray.origin, Rray.direction * legRayDist);
        if (Physics.Raycast(Rray, out hit, legRayDist, layerMask))
        {
            if (!playR && rweight > 0.9f)
            {
                playR = true;
                footSteps.Play();
            }
            Vector3 footPose = hit.point;
            footPose.y += footOffset;
            //print(hit.distance);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, footPose);
            Vector3 forward = Vector3.Cross(anim.GetBoneTransform(HumanBodyBones.RightFoot).right, hit.normal);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(-forward, hit.normal));

        }
        if (rweight < 0.5f)
            playR = false;
        anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, rweight);
        anim.SetIKHintPosition(AvatarIKHint.RightKnee, rightLegHint.position);
        anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, lweight);
        anim.SetIKHintPosition(AvatarIKHint.LeftKnee, leftLegHint.position);
    }

}
