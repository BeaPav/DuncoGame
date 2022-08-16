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
    CharacterController playerController;
    GameObject enemyMesh;
    Transform targetToLookAt;
    Animator enemyAnim;

    ParticleSystem particlesPreAttack;

    [SerializeField] float distToLookAtPlayer = 20f;
    [SerializeField] float distToAttack = 15f;
    [SerializeField] float distTooCloseToAttack = 2f;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float maxAngleToShoot = 45;

    public EnemyState state = EnemyState.CursedPlaced;

    Quaternion startRotation;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float rotGoToStartSpeed = 2f;

    [SerializeField] GameObject bullet;
    private Transform bulletSpawnPoint;
    [SerializeField] float projectileSpeed = 2f;
    //[SerializeField] float projectileOffset;

    float EndAttackTime;
    float EndSubAttackTime;
    [SerializeField] float timeBtwAttack;
    [SerializeField] float timeBtwProjectInAttack;
    private int noProjectilsInAttack = 0;


    [SerializeField] float ShotPrediction;
    [SerializeField] float upFactor;



    [SerializeField] float healOffset = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player-Dunco");
        playerController = player.GetComponent<CharacterController>();
        enemyMesh = transform.Find("Enemy/zombie_huba").gameObject;
        bulletSpawnPoint = transform.Find("Enemy/zombie_huba/BulletSpawnPoint").transform;
        targetToLookAt = null;
        startRotation = enemyMesh.transform.rotation;
        enemyAnim = transform.GetComponent<Animator>();
        EndAttackTime = 0;
        EndSubAttackTime = 0;
        //particlesPreAttack = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        
        DistanceControl();

        if (HealControl())
        {
            //Debug.Log("Heal");
            if(state != EnemyState.Healthy)
                Heal();
            player.GetComponent<PlayerScoreScript>().BounceDown();
            enemyAnim.SetTrigger("isHit");

        }

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

            if (!isAttacking && Time.time - EndAttackTime > timeBtwAttack)
            {
                isAttacking = true;
                noProjectilsInAttack++;
                enemyAnim.SetTrigger("isAttacking");
                EndSubAttackTime = Time.time;

            }
            else if(isAttacking && Time.time - EndSubAttackTime > timeBtwProjectInAttack)
            {
                enemyAnim.SetTrigger("isAttacking");
                EndSubAttackTime = Time.time;
                noProjectilsInAttack++;

                if (noProjectilsInAttack == 3)
                {
                    isAttacking = false;
                    noProjectilsInAttack = 0;
                    EndAttackTime = Time.time;
                    
                }
                
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
        
        Vector3 target = player.transform.position + Vector3.up * upFactor + playerController.velocity.normalized * ShotPrediction;

        Vector3 dir = (target - bulletSpawnPoint.position).normalized;

        if (Vector3.Angle(enemyMesh.transform.forward, dir) < maxAngleToShoot)
        {

            //tri projektily naraz a do stran
            /*
            GameObject proj0 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);
            GameObject proj1 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);
            GameObject proj2 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);

            proj0.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward.normalized * projectileSpeed, ForceMode.Impulse);
            proj1.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(projectileOffset, 0f, 10f).normalized * projectileSpeed, ForceMode.Impulse);
            proj2.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-projectileOffset, 0f, 10f).normalized * projectileSpeed, ForceMode.Impulse);

            */
            //jeden projektil
            GameObject proj0 = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.LookRotation(dir), transform);
            proj0.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward.normalized * projectileSpeed, ForceMode.Impulse);


            //isAttacking = false;
            //EndAttackTime = Time.time;
        }
    }

    public void Heal()
    {
        Debug.Log("Heal");
        state = EnemyState.Healthy;
        enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
        //enemyAnim.SetBool("isHealthy", true);

        player.GetComponent<PlayerScoreScript>().noCollectables++;
        player.GetComponent<PlayerScoreScript>().noCollectablesText.text = player.GetComponent<PlayerScoreScript>().noCollectables.ToString();


    }


    private void LookAtFunc()
    {
        if (targetToLookAt != null)
        {
            Vector3 targPos = player.transform.position - enemyMesh.transform.position;
            targPos.y = 0;
            targPos.Normalize();

            enemyMesh.transform.rotation = Quaternion.LookRotation(
                                                   Vector3.RotateTowards(enemyMesh.transform.forward, targPos, rotSpeed * Time.deltaTime, 0f));
        }
        else
        {
            enemyMesh.transform.rotation = Quaternion.Slerp(enemyMesh.transform.rotation, startRotation, rotGoToStartSpeed * Time.deltaTime);
        }
    }

    private bool HealControl()
    {
        
        if (player.transform.position.y - enemyMesh.transform.position.y < 2.4f && player.transform.position.y - enemyMesh.transform.position.y > 0f)
        {
            if (Mathf.Abs(enemyMesh.transform.position.z - player.transform.position.z) < healOffset &&
               Mathf.Abs(enemyMesh.transform.position.x - player.transform.position.x) < healOffset)
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
            else if (dist < distToAttack && dist > distTooCloseToAttack)
            {
                if (!HealControl())
                {
                    //Debug.Log("CursedAttack");
                    state = EnemyState.CursedAttack;
                }
            }
        }

    }
}
