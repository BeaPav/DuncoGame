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
    Animator sheepAnim;
    NavMeshAgent agent;
    ParticleSystem particles;
    float distToFollow = 7f;
    float distToAttack = 1.9f;
    [SerializeField] bool isAttacking = false;

    public SheepState state = SheepState.CursedPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
        sheepAnim = transform.GetComponent<Animator>();
        particles = transform.Find("Sheep/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();
        if (state == SheepState.CursedFollow)
        {
            agent.destination = player.transform.position;
        }
        else if(state == SheepState.CursedPlaced)
        {
        }
        else if(state == SheepState.CursedAttack && !isAttacking)
        {
            //Debug.Log("startAttack");

            sheepAnim.SetBool("isAttacking", true);
        }
        else if(state == SheepState.Healthy)
        {

        }
    }

    public void PrepareAttack()
    {
        //Debug.Log("prepareAttack");
        isAttacking = true;
        particles.Play(true);
    }

    public void ActivateAttack()
    {
        //Debug.Log("activateAttack");
        transform.Find("Sheep/Cone").GetComponent<Collider>().enabled = true;
    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");
        transform.Find("Sheep/Cone").gameObject.GetComponent<Collider>().enabled = false;
        isAttacking = false;
        sheepAnim.SetBool("isAttacking", false);
        
    }



    private void DistanceControl()
    {
        if (state != SheepState.Healthy)
        {
            float dist = (transform.position - player.transform.position).magnitude;

            if (dist < distToFollow && dist > distToAttack && !isAttacking)
            {
                //Debug.Log("CursedFollow");
                state = SheepState.CursedFollow;
            }
            else if (dist > distToFollow)
            {
                state = SheepState.CursedPlaced;
            }
            else if (dist < distToAttack)
            {
                //Debug.Log("CursedAttack");
                state = SheepState.CursedAttack;
            }
        }

    }
}
