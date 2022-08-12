using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemy : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject enemyMesh;
    Animator enemyAnim;
    NavMeshAgent agent;
    ParticleSystem particlesPreAttack;
    [SerializeField] ParticleSystem particlesDamage;
    Collider damageCollider;
    float distToFollow = 12f;
    float distToAttack = 7f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
        enemyMesh = transform.Find("Enemy").gameObject;
        enemyAnim = transform.GetComponent<Animator>();
        particlesPreAttack = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();
        particlesDamage = transform.Find("ParticleKorgDamage").GetComponent<ParticleSystem>();
        damageCollider = transform.Find("Enemy/DamageCollider").GetComponent<Collider>();
        damageCollider.enabled = false;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();
        if (state == EnemyState.CursedFollow)
        {
            agent.destination = player.transform.position;
        }
        else if (state == EnemyState.CursedPlaced)
        {
            //Debug.Log("Placed");
            DeactivateAttack();
            agent.destination = startPosition;
        }
        else if (state == EnemyState.CursedAttack && !isAttacking)
        {
            //Debug.Log("startAttack");
            enemyAnim.SetBool("isAttacking", true);
        }
        else if (state == EnemyState.Healthy)
        {

        }
    }

    public void PrepareAttack()
    {
        //Debug.Log("prepareAttack");
        isAttacking = true;
        particlesPreAttack.Play(true);
    }

    public void ActivateAttack()
    {
        //Debug.Log("activateAttack");
        
        damageCollider.enabled = true;
        particlesDamage.Play();

    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");

        particlesDamage.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        damageCollider.enabled = false;
        isAttacking = false;
        enemyAnim.SetBool("isAttacking", false);

    }

    public void Heal()
    {
        //Debug.Log("HealStone");
        state = EnemyState.Healthy;
        //enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
        enemyAnim.SetBool("isHealthy", true);

        player.GetComponent<PlayerScoreScript>().noCollectables++;
        player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();

    }



    private void DistanceControl()
    {
        if (state != EnemyState.Healthy)
        {
            float dist = (transform.position - player.transform.position).magnitude;
            //Debug.Log(dist);

            if (dist < distToFollow && dist > distToAttack && !isAttacking)
            {
                //Debug.Log("CursedFollow");
                state = EnemyState.CursedFollow;
            }
            else if (dist > distToFollow)
            {
                state = EnemyState.CursedPlaced;
            }
            else if (dist < distToAttack)
            {
                //Debug.Log("CursedAttack");
                state = EnemyState.CursedAttack;
            }
        }

    }
}
