using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    CursedPlaced,
    CursedFollow,
    CursedAttack,
    Healthy
}



public class SheepEnemy : MonoBehaviour
{
    GameObject player;
    GameObject healPoint;

    SkinnedMeshRenderer enemyRender;

    Animator enemyAnim;
    NavMeshAgent agent;
    ParticleSystem particlesPreAttack;
    ParticleSystem particlesSound;
    AudioSource audio;
    Collider damageCollider;
    float distToFollow;
    [SerializeField] float distToStartFollow = 8f;
    [SerializeField] float distToEndFollow = 18f;
    [SerializeField] float distToAttack = 4f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    float time;
    [SerializeField] float timeBtwAttack;
    [SerializeField] float timeAttackDuration;

    [SerializeField] float healOffset = 1f;

    [SerializeField] Material healthyMaterial;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");

        enemyRender = transform.Find("Enemy/sheep").GetComponent<SkinnedMeshRenderer>();
        healPoint = transform.Find("Enemy/PointToHeal").gameObject;
        enemyAnim = transform.Find("Enemy").gameObject.GetComponent<Animator>();

        particlesPreAttack = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();
        damageCollider = transform.Find("Enemy/DamageCollider").GetComponent<Collider>();
        damageCollider.enabled = false;
        particlesSound = transform.Find("Enemy/ParticleSound").GetComponent<ParticleSystem>();

        distToFollow = distToStartFollow;

        audio = gameObject.GetComponent<AudioSource>();
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
            else
                enemyAnim.SetTrigger("isHit");

            player.GetComponent<PlayerScoreScript>().BounceDown();

        }
        if (state == EnemyState.CursedFollow)
        {
            agent.destination = player.transform.position;
            distToFollow = distToEndFollow;
        }
        else if(state == EnemyState.CursedPlaced)
        {
            distToFollow = distToStartFollow;
        }
        else if(state == EnemyState.CursedAttack)// && !isAttacking)
        {
            //Debug.Log("startAttack");


            //utok cez animaciu so zastavenim, treba pridat do animacie jumping deaktivaciu attack aby fungovalo
            //enemyAnim.SetBool("isAttacking", true);

            //utok bez zastavenia, ovca stale sleduje hraca
            agent.destination = player.transform.position;

            if(!isAttacking && Time.time - time > timeBtwAttack)
            {
                //time = Time.time;
                //Debug.Log(Time.time - time);
                PrepareAttack();
                Invoke("SoundAttack", 1f);
                Invoke("ActivateAttack", 1.2f);
                Invoke("DeactivateAttack", 1.2f + timeAttackDuration);

            }

        }
        else if(state == EnemyState.Healthy)
        {

        }
    }

    public void PrepareAttack()
    {
        if (state != EnemyState.Healthy)
        {
            //Debug.Log("prepareAttack");
            isAttacking = true;
            particlesPreAttack.Play(true);
        }
    }

    public void SoundAttack()
    {
        if (state != EnemyState.Healthy)
        {
            //Debug.Log("playSoundAttack");
            particlesSound.Play(true);
            audio.Play();
        }
    }

    public void ActivateAttack()
    {
        if (state != EnemyState.Healthy)
        {
            //Debug.Log("activateAttack");
            damageCollider.enabled = true;
        }
    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");
        particlesSound.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        damageCollider.enabled = false;
        isAttacking = false;
        audio.Stop();
        //enemyAnim.SetBool("isAttacking", false);
        time = Time.time;
    }

    public void Heal()
    {
        state = EnemyState.Healthy;
        DeactivateAttack();
        enemyRender.material = healthyMaterial;
        enemyAnim.SetBool("isHealthy", true);

        player.GetComponent<PlayerScoreScript>().noCollectables++;
        player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();

    }



    private void DistanceControl()
    {
        if (state != EnemyState.Healthy)
        {
            float dist = (transform.position - player.transform.position).magnitude;

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
}
