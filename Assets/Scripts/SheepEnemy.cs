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
    [SerializeField] GameObject sheepMesh;

    NavMeshAgent agent;
    float distToFollow = 7f;
    float distToAttack = 3f;
    float attackStart;
    [SerializeField] bool isAttacking = false;

    public SheepState state = SheepState.CursedPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();
        if (state == SheepState.CursedFollow)
        {
            isAttacking = false;
            agent.destination = player.transform.position;
            sheepMesh.GetComponent<Animator>().SetBool("isJumping", true);
            sheepMesh.GetComponent<Animator>().SetBool("isAttacking", false);
        }
        else if(state == SheepState.CursedPlaced)
        {
            isAttacking = false;
            sheepMesh.GetComponent<Animator>().SetBool("isJumping", true);
            sheepMesh.GetComponent<Animator>().SetBool("isAttacking", false);
        }
        else if(state == SheepState.CursedAttack)
        {
            sheepMesh.GetComponent<Animator>().SetBool("isJumping", false);
            sheepMesh.GetComponent<Animator>().SetBool("isAttacking", true);
            if (!isAttacking)
            {
                isAttacking = true;
                attackStart = Time.time;
                sheepMesh.transform.Find("ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>().Play(true);
            }
            else if(Time.time - attackStart > 2f)
            {
                isAttacking = false;
            }

        }
        else if(state == SheepState.Healthy)
        {

        }
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
