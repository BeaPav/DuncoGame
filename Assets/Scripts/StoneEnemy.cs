using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemy : MonoBehaviour
{
    GameObject player;

    GameObject healPoint;
    Animator enemyAnim;
    NavMeshAgent agent;

    ParticleSystem particlesPreAttack;
    ParticleSystem particleRising;
    ParticleSystem particlesDamage;
    Collider damageCollider;
    Collider damageRisingCollider;

    float distToFollow = 12f;
    float distToAttack = 7f;

    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isRising = false;

    [SerializeField] float healOffset = 0.6f;

    public EnemyState state = EnemyState.CursedPlaced;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");

        healPoint = transform.Find("Enemy/PointToHeal").gameObject;

        enemyAnim = transform.GetComponent<Animator>();

        particlesPreAttack = transform.Find("Enemy/ParticleExplosionKorg/SmallerParticles").GetComponent<ParticleSystem>();
        particlesDamage = transform.Find("ParticleKorgDamage").GetComponent<ParticleSystem>();
        particleRising = transform.Find("Enemy/ParticleRising").GetComponent<ParticleSystem>();
        damageCollider = transform.Find("Enemy/DamageCollider").GetComponent<Collider>();
        damageCollider.enabled = false;
        damageRisingCollider = transform.Find("Enemy/DamageRisingCollider").GetComponent<Collider>();
        damageRisingCollider.enabled = false;

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();

        if (HealControl())
        {
            //Debug.Log("Heal");
            if (state != EnemyState.Healthy)
                Heal();
            player.GetComponent<PlayerScoreScript>().BounceDown();

        }

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

    public void ActivateRisingAttack()
    {
        //Debug.Log("risingAttack");
        isRising = true;
        damageRisingCollider.enabled = true;
        particleRising.Play();
    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");

        particlesDamage.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleRising.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        damageCollider.enabled = false;
        damageRisingCollider.enabled = false;
        isAttacking = false;
        isRising = false;
        enemyAnim.SetBool("isAttacking", false);

    }

    public void Heal()
    {
        if (!isRising)
        {
            //Debug.Log("HealStone");
            state = EnemyState.Healthy;
            //enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
            enemyAnim.SetBool("isHealthy", true);

            player.GetComponent<PlayerScoreScript>().noCollectables++;
            player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();
        }

    }


    private bool HealControl()
    {
        //Debug.Log(player.transform.position.y > healPoint.transform.position.y);
        if (player.transform.position.y - healPoint.transform.position.y < 0.4f && player.transform.position.y - healPoint.transform.position.y > 0f)
        {
            if (Mathf.Abs(healPoint.transform.position.z - player.transform.position.z) < healOffset &&
               Mathf.Abs(healPoint.transform.position.x - player.transform.position.x) < healOffset)
            {
                return true;
            }
        }
        return false;
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
