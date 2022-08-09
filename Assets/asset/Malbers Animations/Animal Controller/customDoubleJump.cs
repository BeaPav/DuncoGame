using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customDoubleJump : MonoBehaviour
{
    public Rigidbody rb;

    public void Update()
    {
        rb.AddForce(Vector3.up*Time.deltaTime*100);
    }
}
