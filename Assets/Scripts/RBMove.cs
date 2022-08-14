using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMove : MonoBehaviour
{
    public float speed = 10f; //Controls velocity multiplier
    public float jump = 10f; //Controls velocity multiplier
    public float rotateSpeed = 10f; //Controls velocity multiplier
    Rigidbody rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Vector3 movement;
    Transform camera;
    float smoothTurnVelocity;
    [SerializeField] float smoothTurnTime = 0.2f;
    [SerializeField] LayerMask groundLayer;
    int jumps;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Vertical"),0, Input.GetAxis("Horizontal") * -1);
        Jump();
        Ground();
    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector3 direction)
    {
        // Convert direction into Rigidbody space.
        direction = rb.rotation * direction;
        Rotation();
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void Rotation()
    {
        Quaternion rotation = Quaternion.Lerp(transform.transform.rotation *= Quaternion.Euler(0, -20, 0), camera.transform.rotation, rotateSpeed * Time.deltaTime);
        rotation.x = 0;
        rotation.z = 0;
        //rotation *= Quaternion.Euler(0, 50, 0);
        transform.rotation = rotation;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }

    void Ground()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.3f, groundLayer))
        {
            jumps = 1;
        }
    }
}
