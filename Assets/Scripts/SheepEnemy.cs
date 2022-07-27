using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepEnemy : MonoBehaviour
{
    GameObject targetToFollow;
    NavMeshAgent agent;
    public bool isCursed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isCursed.ToString());
        if (targetToFollow != null && isCursed)
        {
            agent.destination = targetToFollow.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("triggeredToFollow");
            targetToFollow = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            targetToFollow = null;
        }
    }
}
