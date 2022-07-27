using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepEnemy : MonoBehaviour
{
    GameObject targetToFollow;
    GameObject player;
    NavMeshAgent agent;
    public bool isCursed;
    float distToStartFollow = 7f;
    float distToEndFollow = 8f;

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
        if (targetToFollow != null && isCursed)
        {
            agent.destination = targetToFollow.transform.position;
        }
    }
    private void DistanceControl()
    {
        float dist = (transform.position - player.transform.position).magnitude;

        if (dist < distToStartFollow)
        {
            targetToFollow = player;
        }
        else if(dist > distToEndFollow)
        {
            targetToFollow = null;
        }

    }

}
