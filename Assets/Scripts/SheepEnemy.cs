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
    NavMeshAgent agent;
    float distToStartFollow = 7f;
    float distToEndFollow = 8f;

    public SheepState state = SheepState.CursedPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player-Dunco");
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
        else if(state == SheepState.Healthy)
        {

        }
    }



    private void DistanceControl()
    {
        float dist = (transform.position - player.transform.position).magnitude;

        if (dist < distToStartFollow && state != SheepState.Healthy)
        {
            state = SheepState.CursedFollow;
        }
        else if(dist > distToEndFollow && state != SheepState.Healthy)
        {
            state = SheepState.CursedPlaced;
        }

    }

}
