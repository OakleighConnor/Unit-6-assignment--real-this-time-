using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    Player playerScript;
    Rigidbody rb;
    Vector3 targetPos;
    Animator anim;

    public float seperation;

    public NavMeshAgent enemy;
    public Transform player;

    public float movementRange = 5;
    public float attackRange = 1;

    public bool moving = true;
    public bool stunned = false;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Third Person Player").GetComponent<Player>();
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks the distance between the player and the enemy. If the player is close enough to the enemy it chases the player
        seperation = Vector3.Distance(transform.position, player.position);
        if (moving)
        {
            rb.constraints = RigidbodyConstraints.None;
            if (seperation <= movementRange)
            {
                if (seperation >= 1f)
                {
                    float targetPos1 = player.position.x;
                    float targetPos2 = player.position.z;
                    targetPos = new Vector3(targetPos1 - 0.5f, -0.0200001f, targetPos2);
                    enemy.SetDestination(targetPos);
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    anim.SetTrigger("attack");
                    moving = false;
                }
            }
        }
        //Checks if the player has reached the target location
        if (Vector3.Distance(transform.position, targetPos) <= 1)
        {
            anim.SetBool("walking", false);
        }
        else
        {
            anim.SetBool("walking", true);
        }

    }
    public void TakeDamage()
    {
        moving = false;
        anim.SetTrigger("death");
    }
}