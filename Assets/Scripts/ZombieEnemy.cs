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

    [SerializeField] float distToLookAtPlayer = 20f;
    [SerializeField] float distToAttack = 15f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    Quaternion startRotation;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float rotGoToStartSpeed = 2f;

    [SerializeField] GameObject bullet;
    private Transform bulletSpawnPoint;
    [SerializeField] float projectileSpeed = 2f;
    [SerializeField] float projectileOffset;

    float time;
    [SerializeField] float timeBtwAttack;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player-Dunco");
        enemyShootingMesh = transform.Find("Enemy/zombie_shooter").gameObject;
        bulletSpawnPoint = transform.Find("Enemy/zombie_shooter/BulletSpawnPoint").transform;
        targetToLookAt = null;
        startRotation = enemyShootingMesh.transform.rotation;
        enemyAnim = transform.GetComponent<Animator>();
        time = 0;
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
        else if (state == EnemyState.CursedAttack)
        {
            //Debug.Log("startAttack");
            targetToLookAt = player.transform;

            if (!isAttacking && Time.time - time > timeBtwAttack)
            {
                isAttacking = true;
                enemyAnim.SetTrigger("isAttacking");
            }
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
        Vector3 dir = (player.transform.position - bulletSpawnPoint.position).normalized;

        GameObject proj0 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);
        GameObject proj1 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);
        GameObject proj2 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);

        proj0.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward.normalized * projectileSpeed, ForceMode.Impulse);
        proj1.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(projectileOffset, 0f, 10f).normalized * projectileSpeed, ForceMode.Impulse);
        proj2.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-projectileOffset, 0f, 10f).normalized * projectileSpeed, ForceMode.Impulse);

        
        isAttacking = false;
        time = Time.time;
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
            enemyShootingMesh.transform.rotation = Quaternion.Slerp(enemyShootingMesh.transform.rotation, startRotation, rotGoToStartSpeed * Time.deltaTime);
        }
    }


    private void DistanceControl()
    {
        if (state != EnemyState.Healthy)
        {
            Vector3 distVect = transform.position - player.transform.position;
            distVect.y = 0;
            float dist = distVect.magnitude;

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
