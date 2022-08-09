using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    private float startTime;

    private void Awake()
    {
        //Debug.Log("Awake");
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > 2f)
        {
            //Debug.Log("Destroy");
            Destroy(this.gameObject, 0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Projectile")
        {
            //Debug.Log("DestroyTrigger");
            Destroy(this.gameObject, 0.2f);
        }
    }
}
