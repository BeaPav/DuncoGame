using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum SheepState
{
    CursedPlaced,
    CursedFollow,
    CursedAttack,
    Healthy
}



public class SheepEnemy : MonoBehaviour
{
    GameObject player;
    GameObject sheepMesh;
    Animator sheepAnim;
    NavMeshAgent agent;
    float distToFollow = 7f;
    float distToEndFollow = 10f;
    float distToAttack = 3f;
    float attackStart;
    [SerializeField] bool isAttacking = false;

    public SheepState state = SheepState.CursedPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
        sheepMesh = transform.Find("Sheep").gameObject;
        sheepAnim = sheepMesh.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();
        if (state == SheepState.CursedFollow)
        {
            agent.destination = player.transform.position;
            Jump();
        }
        else if(state == SheepState.CursedPlaced)
        {
            Jump();
        }
        else if(state == SheepState.CursedAttack)
        {
            Attack();
        }
        else if(state == SheepState.Healthy)
        {

        }
    }

    private void Attack()
    {
        sheepAnim.SetBool("isJumping", false);
        sheepAnim.SetBool("isAttacking", true);
        if (!isAttacking)
        {
            isAttacking = true;
            attackStart = Time.time;
            sheepMesh.transform.Find("ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>().Play(true);
        }
        else if (Time.time - attackStart > 1.5f)
        {
            isAttacking = false;
        }
    }

    private void Jump()
    {
        isAttacking = false;
        sheepAnim.SetBool("isJumping", true);
        sheepAnim.SetBool("isAttacking", false);
    }



    private void DistanceControl()
    {
        float dist = (transform.position - player.transform.position).magnitude;

        if (state != SheepState.Healthy)
        {
            if (dist < distToFollow && dist > distToAttack && !isAttacking)
            {
                state = SheepState.CursedFollow;
            }
            else if (dist > distToFollow)
            {
                state = SheepState.CursedPlaced;
            }
            else if (dist < distToAttack)
            {
                state = SheepState.CursedAttack;
            }
        }

    }
}
