using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEscape : MonoBehaviour
{
    Vector3 targetDir;
    Vector3 startPos;
    Vector3 moveDir;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float maxDistance;

    bool isHit = false;

    Rigidbody rb;

    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player-Dunco");
        
        startPos = transform.position;
        targetDir = (startPos - player.transform.position);
        targetDir.y = 0;
        targetDir = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * targetDir;


        Vector3.Normalize(targetDir);

        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DistanceControl() && !isHit)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotSpeed * Time.deltaTime);
            moveDir = (transform.forward).normalized;
            //rb.AddForce(moveDir * moveSpeed, ForceMode.Force);
            rb.velocity = moveDir * moveSpeed;
            
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        isHit = true;
    }


    bool DistanceControl()
    {
        float dist = (transform.position - startPos).magnitude;
        if (dist < maxDistance) return true;
        return false;
    }
    
}
