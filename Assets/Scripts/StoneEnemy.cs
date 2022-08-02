using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemy : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject enemyMesh;
    //Animator enemyAnim;
    NavMeshAgent agent;
    [SerializeField] ParticleSystem particles;
    //Collider damageCollider;
    float distToFollow = 7f;
    float distToAttack = 3f;
    [SerializeField] bool isAttacking = false;

    public EnemyState state = EnemyState.CursedPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
        enemyMesh = transform.Find("Enemy").gameObject;
        //enemyAnim = transform.GetComponent<Animator>();
        particles = transform.Find("Enemy/ParticleExplosion/SmallerParticles").GetComponent<ParticleSystem>();
        //damageCollider = transform.Find("Enemy/HitCollider").GetComponent<Collider>();
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
        }
        else if (state == EnemyState.CursedAttack && !isAttacking)
        {
            //Debug.Log("startAttack");

            //enemyAnim.SetBool("isAttacking", true);
        }
        else if (state == EnemyState.Healthy)
        {

        }
    }


    /// <summary>
    /// //////////////////////////////////////////////utok nejde lebo na ovci je spusteny z animu
    /// 
    /// </summary>
    public void PrepareAttack()
    {
        Debug.Log("prepareAttack");
        isAttacking = true;
        particles.Play(true);
    }

    public void ActivateAttack()
    {
        Debug.Log("activateAttack");
        
        //damageCollider.enabled = true;


    }

    public void DeactivateAttack()
    {
        Debug.Log("deactivateAttack");
        
        //damageCollider.enabled = false;
        isAttacking = false;
        //enemyAnim.SetBool("isAttacking", false);

    }

    public void Heal()
    {
        Debug.Log("HealStone");
        state = EnemyState.Healthy;
        enemyMesh.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0.8f, 1f));
        //enemyAnim.SetBool("isHealthy", true);

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
}
