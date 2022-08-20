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
    ParticleSystem particlesDamage;
    Collider damageCollider;

    float distToFollow = 12f;
    float distToAttack = 7f;

    [SerializeField] bool isAttacking = false;

    [SerializeField] float healOffset;
    [SerializeField] float bounceStrength;
    bool bouncing;

    public EnemyState state = EnemyState.CursedPlaced;

    private Vector3 startPosition;

    [SerializeField] AudioSource audioAttack;
    [SerializeField] AudioSource audioHit;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");

        healPoint = transform.Find("Enemy/PointToHeal").gameObject;

        enemyAnim = transform.GetComponent<Animator>();

        particlesPreAttack = transform.Find("Enemy/ParticleExplosionKorg/SmallerParticles").GetComponent<ParticleSystem>();
        particlesDamage = transform.Find("ParticleKorgDamage").GetComponent<ParticleSystem>();
        damageCollider = transform.Find("Enemy/DamageCollider").GetComponent<Collider>();
        damageCollider.enabled = false;

        startPosition = transform.position;

        

        bouncing = false;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceControl();

        if (HealControl())
        {
            //Debug.Log("Heal");
            if (state != EnemyState.Healthy)
            {
                Heal();
                audioHit.Play();
                bouncing = true;
                Invoke("BouncingFalse", 1f);
                player.GetComponent<PlayerScoreScript>().BounceDown(bounceStrength);
                enemyAnim.SetTrigger("isHit");
            }

            if (!bouncing)
            {
                audioHit.Play();
                bouncing = true;
                Invoke("BouncingFalse", 1f);
                player.GetComponent<PlayerScoreScript>().BounceDown(bounceStrength);
                //enemyAnim.SetTrigger("isHit");
            }

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


    void BouncingFalse()
    {
        bouncing = false;
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
        audioAttack.Play();

    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");

        particlesDamage.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        damageCollider.enabled = false;
        isAttacking = false;
        enemyAnim.SetBool("isAttacking", false);
        audioAttack.Stop();

    }

    public void Heal()
    {
        /*
        if (!isRising)
        {
            //Debug.Log("HealStone");
            state = EnemyState.Healthy;
            //enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
            enemyAnim.SetBool("isHealthy", true);

            player.GetComponent<PlayerScoreScript>().noCollectables++;
            player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();
        }
        */

        //Debug.Log("HealStone");
        state = EnemyState.Healthy;
        
        enemyAnim.SetBool("isHealthy", true);
        transform.Find("Enemy/Cube.001").gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        player.GetComponent<PlayerScoreScript>().noCollectables++;
        player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();

    }

    public void CurseAgain()
    {
        state = EnemyState.CursedPlaced;

        enemyAnim.SetBool("isHealthy", false);
        enemyAnim.SetBool("isAttacking", false);
        enemyAnim.SetTrigger("cursedAgain");
        transform.Find("Enemy/Cube.001").gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }


    private bool HealControl()
    {
        Vector3 distControl = player.transform.position - healPoint.transform.position;
        if (distControl.y < 0.4f && distControl.y > 0f)
        {
            distControl.y = 0f;
            if (distControl.magnitude < healOffset)
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
