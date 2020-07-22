using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class insects : MonoBehaviour
{
    NavMeshAgent nMesh;
    public float currHealth;
    public float maxHealth=100;
    public bool followPlayer;
    playerMovement plScript;
    public LayerMask layerMask;
    float lastAttack;
    float attackInterval=3;
    bool dead;
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        nMesh =GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth < 0 && !dead)
        {
            dead = true;
            gameObject.AddComponent<Rigidbody>();
            nMesh.enabled = false;
            //Destroy(gameObject);
        }
        else if (followPlayer && nMesh!=null && !dead)
        {
            nMesh.speed = 1;
            nMesh.SetDestination(enemyCommon.player.transform.position);
            Vector3 dist =(enemyCommon.player.transform.position-transform.position);
            if (dist.magnitude < 0.6f)
            {
                if (lastAttack + attackInterval < Time.time || lastAttack<=0.1f)
                {
                    //if (!Physics.Linecast(transform.position, enemyCommon.player.transform.position,layerMask))
                    {
                        enemyCommon.plScript.ShoutItHurts = true;
                        lastAttack = Time.time;
                        enemyCommon.plScript.currHealth -= 3;
                    }
                }
            }
        }
        else if(nMesh!=null && !dead)
        {
            nMesh.speed = 0;
            nMesh.SetDestination(transform.position);
        }
    }

   
}
