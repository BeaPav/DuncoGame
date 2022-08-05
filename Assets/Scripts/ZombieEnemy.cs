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
    GameObject enemyShootingMesh;
    Transform targetToLookAt;
    Animator enemyAnim;

    ParticleSystem particlesPreAttack;

    float distToLookAtPlayer = 12f;
    float distToAttack = 10f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    Quaternion startRotation;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float rotGoToStartRotSpeed = 2f;

    [SerializeField] GameObject bullet;
    private Transform bulletSpawnPoint;
    [SerializeField] float force = 2000f;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player-Dunco");
        enemyShootingMesh = transform.Find("Enemy/zombie_shooter").gameObject;
        bulletSpawnPoint = transform.Find("Enemy/zombie_shooter/BulletSpawnPoint").transform;
        targetToLookAt = null;
        startRotation = enemyShootingMesh.transform.rotation;
        enemyAnim = transform.GetComponent<Animator>();
        //particlesPreAttack = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        
        DistanceControl();
        if (state == EnemyState.CursedFollow)
        {
            targetToLookAt = player.transform;
            enemyAnim.SetBool("isAttacking", false);
        }
        else if (state == EnemyState.CursedPlaced)
        {
            targetToLookAt = null;
            //enemyAnim.SetBool("isAttacking", false);
        }
        else if (state == EnemyState.CursedAttack && !isAttacking)
        {
            //Debug.Log("startAttack");
            targetToLookAt = player.transform;
            enemyAnim.SetBool("isAttacking", true);
        }
        else if (state == EnemyState.Healthy)
        {
            targetToLookAt = null;
        }

        LookAtFunc();
    }

    /*
    public void PrepareAttack()
    {
        //Debug.Log("prepareAttack");
        isAttacking = true;
        particlesPreAttack.Play(true);
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
    */

    public void Shoot()
    {
        GameObject proj = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity, transform);
        Vector3 dir = (player.transform.position - bulletSpawnPoint.position);
        dir.y = 0f;
        proj.GetComponent<Rigidbody>().AddForce(dir * force,ForceMode.Impulse);
    }

    public void Heal()
    {
        state = EnemyState.Healthy;
        enemyShootingMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
        //enemyAnim.SetBool("isHealthy", true);

    }

    
    private void LookAtFunc()
    {
        if (targetToLookAt != null)
        {
            Vector3 targPos = player.transform.position - enemyShootingMesh.transform.position;
            targPos.y = 0;
            targPos.Normalize();

            enemyShootingMesh.transform.rotation = Quaternion.LookRotation(
                                                   Vector3.RotateTowards(enemyShootingMesh.transform.forward, targPos, rotSpeed * Time.deltaTime, 0f));
        }
        else
        {
            enemyShootingMesh.transform.rotation = Quaternion.Slerp(enemyShootingMesh.transform.rotation, startRotation, rotGoToStartRotSpeed * Time.deltaTime);
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
