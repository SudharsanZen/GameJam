using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAi : MonoBehaviour
{
    public List<Transform> paths;
    public LayerMask layerMask;

    public float runSpeed=1;
    public float walkSpeed=2;
    float timeSinceLastSeen=0;
    float coolDownInterval=4;
    public float enemyFieldOfView=80;
    public float enemyViewRange=10;
    public float attackRange=1f;
    GameObject player;
    NavMeshAgent nMesh;
    
    Animator anim;
    bool playerSpotted=false;
    bool inSight=false;
    bool attack=false;
    bool playerDead=false;
    bool dead=false;
    bool scout=true;
    bool attackOver=false;
    int currPath = 0;
    float nextScoutStartTime;
    float idleTimeAfterWalk = 1;
    bool cameNearPath = false;
    Vector3 lastDestination;
    // Start is called before the first frame update
    void Start()
    {
        nMesh =GetComponent<NavMeshAgent>();
        nMesh.SetDestination(paths[0].position);
        nMesh.speed =walkSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        attackOver = anim.GetBool(minionStatics.attackOver);
        sight();
        setParam();
        doEnemyActions();
    }

    void doEnemyActions()
    {
        if (playerSpotted)
        {
            //print((player.transform.position - transform.position).magnitude);
            if ((player.transform.position - transform.position).magnitude < attackRange)
            {
                attack = true;
                attackPlayer();
            }
            else if (attackOver)
            {

                followPlayer();
            }
            

        }
        else
        {

            followPath();
        }
    }
    void followPlayer()
    {
        attack = false;
        nMesh.speed = runSpeed;
        lastDestination = player.transform.position;
        lastDestination.y =transform.position.y;
        if(lastDestination != nMesh.destination)
            nMesh.SetDestination(lastDestination);
       
    }
    void attackPlayer()
    {
        attack = true;
        nMesh.speed = 0;

        
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

    void sight()
    {
        //checks if there is any obstacles between the player and the mosnter
        //and also checks if the player is in the field of view of the monster and within the range
        Vector3 offset =new Vector3(0,1,0);
        Debug.DrawLine(transform.position+offset,player.transform.position+offset,Color.green);

        Debug.DrawRay(transform.position + offset, (Quaternion.Euler(0,enemyFieldOfView,0)* transform.forward)*enemyViewRange , Color.blue);
        Debug.DrawRay(transform.position + offset, (Quaternion.Euler(0,-enemyFieldOfView,0)* transform.forward)*enemyViewRange , Color.blue);

        float angleBetweenPlayer =Vector3.SignedAngle(transform.forward,(player.transform.position-transform.position).normalized,Vector3.up);
        //print("Angle: "+angleBetweenPlayer);
        if (!Physics.Linecast(transform.position + offset, player.transform.position+offset, layerMask) && (Mathf.Abs(angleBetweenPlayer)<enemyFieldOfView||Input.GetButton(InputStatics.fire)))
        {
            lastDestination = player.transform.position;
            timeSinceLastSeen = Time.time;
            playerSpotted = true;
        }
        else
        {

            if (timeSinceLastSeen + coolDownInterval < Time.time)
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

        if (dist.magnitude < 1)
        {
        
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, anim.GetFloat(minionStatics.attack1Weight));
      

            anim.SetIKPosition(AvatarIKGoal.LeftHand, player.transform.position + new Vector3(0, 0.5f, 0));
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (attack)
            ikHandling();
    }
}
