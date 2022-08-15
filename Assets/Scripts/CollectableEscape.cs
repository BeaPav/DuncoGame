using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEscape : MonoBehaviour
{
    [SerializeField] Vector3 targetDir;
    Vector3 startPos;
    [SerializeField] Vector3 moveDir;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float maxDistance;

    bool isHit = false;

    [SerializeField] Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceControl() && !isHit)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotSpeed * Time.deltaTime);
            moveDir = (transform.forward).normalized;
            
            rb.AddForce(moveDir * moveSpeed, ForceMode.Force);
            //rb.velocity = moveDir * moveSpeed;

        }
        else
        {
            rb.velocity = Vector3.zero;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Collectable" && other.tag != "Projectile" && other.tag != "Terrain" 
            && other.tag != "Korg" && other.tag != "DamageStone" && other.tag != "DamageSound" )
        {
            isHit = true;
            
        }
    }

    public void CreateTargetDir(Vector3 playerPos)
    {
        //Debug.Log(playerPos);
        startPos = transform.position;

        targetDir = startPos - playerPos;

        targetDir.y = 0;
        targetDir = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * targetDir;
        GetComponent<Rigidbody>().AddForce(-1f * Vector3.up);
        Vector3.Normalize(targetDir);
    }

    private bool DistanceControl()
    {
        float dist = (transform.position - startPos).magnitude;
        if (dist < maxDistance) return true;
        return false;
    }
    
}
