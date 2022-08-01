using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    /*
     TODO
        - spravit komentare
        - vyjebat Cursor.lockState a dat do nejakeho game manageru
        - prerobit jump na new input system
     */
    public InputAction playerInputs;

    [SerializeField] float smoothTurnTime = 0.2f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float movementSpeed = 3f;

    float smoothTurnVelocity;
    public float ySpeed;

    public bool move;
    public float gravityMultiplyer = 2f;

    Transform camera;

    CharacterController charControler;

    [SerializeField] GameObject model, frontPoint, backPoint;
    public float fix;
    

    private void Start()
    {
        playerInputs.Enable();

        charControler = GetComponent<CharacterController>();
        camera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // neskorej vyjebat
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
            Movement();
    }

    void Movement()
    {
        Vector2 inputDirection = playerInputs.ReadValue<Vector2>();

        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
        Vector3 moveDirection = Rotation(direction) * direction.magnitude;

        ySpeed = Gravity(ySpeed);
        ySpeed = Jump(ySpeed);
        moveDirection.y = ySpeed;

        copyTerrain();

        //Debug.Log(moveDirection.y);
        charControler.Move(moveDirection * Time.deltaTime);
    }

    float Jump(float ySpeed)
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return ySpeed;
        }

        return jumpSpeed;
    }

    float Gravity(float ySpeed)
    {
        if (!charControler.isGrounded)
        {
            gravityMultiplyer *= 1.03f;
            return ySpeed += Physics.gravity.y * Time.deltaTime * gravityMultiplyer;
            
        }

        gravityMultiplyer = 1f;
        return -0.5f;
    }

    //pohyb, ze mys kontroluje aj pohyb postavy
    Vector3 Rotation(Vector3 Direction)
    {
        if (Direction.magnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle - 90, ref smoothTurnVelocity, smoothTurnTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            return moveDirection.normalized * movementSpeed;
        }

        return Vector3.zero;
    }

    void copyTerrain() //premenovat
    {
        RaycastHit Fhit, Bhit;
        if (Physics.Raycast(frontPoint.transform.position, frontPoint.transform.TransformDirection(Vector3.down), out Fhit, Mathf.Infinity) && Physics.Raycast(backPoint.transform.position, backPoint.transform.TransformDirection(Vector3.down), out Bhit, Mathf.Infinity))
        {
            Vector3 upright = Vector3.Cross(model.transform.right, -(Fhit.point - Bhit.point).normalized);
            Quaternion angel = Quaternion.LookRotation(Vector3.Cross(model.transform.right, upright));

            
            model.transform.rotation = angel;
            //Debug.Log(angel);
            /*
             * 
             *  Vector3 upright = Vector3.Cross(model.transform.right, -(Fhit.point - Bhit.point).normalized);
             *   Vector3 angel = Quaternion.LookRotation(Vector3.Cross(model.transform.right, upright)).eulerAngles;
             *   model.transform.eulerAngles = angel;
             *   Debug.Log(angel);
             */
        }
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
}

