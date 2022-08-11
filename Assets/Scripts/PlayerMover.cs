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
    [SerializeField] float slopeSpeedChange = 700f;

    float smoothTurnVelocity;
    public float ySpeed;

    public bool move;
    [SerializeField] float gravityStartCondition = 2f;
    [SerializeField] float gravityMultiplConst = 1.3f;
    float gravityFinal;

    Transform camera;

    CharacterController charControler;

    [SerializeField] GameObject model, frontPoint, backPoint;

    int jumps = 0;
    public float fix;

    Animator anim;
    

    private void Start()
    {
        playerInputs.Enable();
        gravityFinal = gravityStartCondition;

        charControler = GetComponent<CharacterController>();
        camera = Camera.main.transform;
        
        /*
        anim = transform.Find("pivot/test").GetComponent<Animator>();
        model = transform.Find("pivot/test").gameObject;
        */
        
        anim = transform.Find("pivot/Dunco").GetComponent<Animator>();
        model = transform.Find("pivot/Dunco").gameObject;
        
        
        Cursor.lockState = CursorLockMode.Locked; // neskorej vyjebat
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(charControler.isGrounded);
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

        
        

        //Debug.Log(moveDirection.y);
        charControler.Move(moveDirection * Time.deltaTime);
    }

    float Jump(float ySpeed)
    {
        if (!Input.GetKeyDown(KeyCode.Space) || jumps >= 2)
        {
            //copyTerrain();
            return ySpeed;
        }
        anim.SetTrigger("isJumping");
        jumps++;
        gravityFinal = gravityStartCondition;
        //Debug.Log("jump");
        return jumpSpeed;
    }

    float Gravity(float ySpeed)
    {
        if (!charControler.isGrounded)
        {
            gravityFinal *= gravityMultiplConst;
            return ySpeed += Physics.gravity.y * Time.deltaTime * gravityFinal;
            
        }
        
        jumps = 0;
        gravityFinal = gravityStartCondition;
        return -0.5f;
    }

    //pohyb, ze mys kontroluje aj pohyb postavy
    Vector3 Rotation(Vector3 Direction)
    {
        if (Direction.magnitude > 0.01f)
        {
            anim.SetBool("isRunning", true);

            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle - 90, ref smoothTurnVelocity, smoothTurnTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            return moveDirection.normalized * movementSpeed;
        }

        anim.SetBool("isRunning", false);
        return Vector3.zero;
    }

    void copyTerrain() //premenovat
    {
        RaycastHit Fhit, Bhit;
        if (Physics.Raycast(frontPoint.transform.position, frontPoint.transform.TransformDirection(Vector3.down), out Fhit, 0.5f) && Physics.Raycast(backPoint.transform.position, backPoint.transform.TransformDirection(Vector3.down), out Bhit, 0.5f))
        {

            Vector3 point1 = frontPoint.transform.position;
            Vector3 point2 = Bhit.point;
            int nasobitel = 1;
            if (Fhit.point.y < Bhit.point.y)
            {
                point1 = backPoint.transform.position;
                point2 = Fhit.point;
                nasobitel = -1;
            }
            
            Vector3 point3 = new Vector3(point1.x, point2.y, point1.z);

            Vector3 angle = model.transform.eulerAngles;//Vector3.Angle(point3-point2,point1-point2) 
            angle.z = Mathf.LerpAngle(angle.z, Vector3.Angle(point3 - point2, point1 - point2) * nasobitel, slopeSpeedChange * Time.deltaTime);
            model.transform.eulerAngles = angle;

            /*
            Debug.Log("=====");
            Debug.Log(frontPoint.transform.position);
            Debug.Log(Fhit.point);
            Debug.Log("=");
            Debug.Log(backPoint.transform.position);
            Debug.Log(Bhit.point);
            Debug.Log("-");
            Debug.Log(angle);
            */

        /*Vector3 upright = Vector3.Cross(model.transform.right, -(Fhit.point - Bhit.point).normalized);
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

