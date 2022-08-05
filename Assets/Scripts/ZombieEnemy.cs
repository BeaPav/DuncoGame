using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
public enum EnemyState
{
    CursedPlaced, == neaktivny
    CursedFollow, == zdvihne sa a sleduje hraca, zameriava
    CursedAttack, == striela
    Healthy
}
*/

public class ZombieEnemy : MonoBehaviour
{
    GameObject player;
    GameObject enemyMesh;
    Transform targetToLookAt;
    //Animator enemyAnim;

    ParticleSystem particlesPreAttack;

    float distToLookAtPlayer = 7f;
    float distToAttack = 6f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    Quaternion startRotation;
    [SerializeField] float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player-Dunco");
        enemyMesh = transform.Find("Enemy").gameObject;
        targetToLookAt = transform;
        startRotation = transform.rotation;
        rotationSpeed = 10f;
        //enemyAnim = transform.GetComponent<Animator>();
        //particlesPreAttack = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        
        DistanceControl();
        if (state == EnemyState.CursedFollow)
        {
            targetToLookAt = player.transform;
        }
        else if (state == EnemyState.CursedPlaced)
        {
            targetToLookAt = null;
        }
        else if (state == EnemyState.CursedAttack && !isAttacking)
        {
            //Debug.Log("startAttack");
            targetToLookAt = player.transform;
            //enemyAnim.SetBool("isAttacking", true);
        }
        else if (state == EnemyState.Healthy)
        {
            targetToLookAt = null;
        }

        LookAtFunc();
    }

    public void PrepareAttack()
    {
        //Debug.Log("prepareAttack");
        isAttacking = true;
        particlesPreAttack.Play(true);
    }

    public void SoundAttack()
    {
        //Debug.Log("PlaySoundAttack");
    }

    public void ActivateAttack()
    {
        //Debug.Log("activateAttack");
    }

    public void DeactivateAttack()
    {
        //Debug.Log("deactivateAttack");
        isAttacking = false;

    }

    public void Heal()
    {
        state = EnemyState.Healthy;
        enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
        //enemyAnim.SetBool("isHealthy", true);

    }

    
    private void LookAtFunc()
    {
        if (targetToLookAt != null)
        {
            Vector3 targPos = player.transform.position - transform.position;
            targPos.y = 0;
            targPos.Normalize();

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targPos, rotationSpeed * Time.deltaTime, 0f));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void DistanceControl()
    {
        if (state != EnemyState.Healthy)
        {
            float dist = (transform.position - player.transform.position).magnitude;

            if (dist < distToLookAtPlayer && dist > distToAttack && !isAttacking)
            {
                //Debug.Log("CursedFollow");
                state = EnemyState.CursedFollow;
            }
            else if (dist > distToLookAtPlayer)
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
