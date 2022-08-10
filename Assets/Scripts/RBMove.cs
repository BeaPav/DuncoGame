using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMove : MonoBehaviour
{
    public float speed = 10f; //Controls velocity multiplier
    public float jump = 10f; //Controls velocity multiplier
    Rigidbody rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        rb.MovePosition(transform.position + m_Input * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jump);
        }
    }
}
