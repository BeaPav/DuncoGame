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
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float movementSpeed = 3f;

    float smoothTurnVelocity;
    float ySpeed;

    public bool move;

    Transform camera;

    CharacterController charControler;

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
        Debug.Log(moveDirection.y);
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
            return ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        return -0.5f;
    }

    //pohyb, ze mys kontroluje aj pohyb postavy
    Vector3 Rotation(Vector3 Direction)
    {
        float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        return moveDirection.normalized * movementSpeed;
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
}

