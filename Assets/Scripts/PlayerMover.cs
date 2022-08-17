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

    ParticleSystem barkParticles;

    int jumps = 0;
    public float fix;

    Animator anim;

    public float barkTime;
    public float barkCoolDown;
    public GameObject bark;
    private float barkCounting;
    private float barkCooldownCounting;

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
        barkParticles = transform.Find("pivot/Dunco/Bark").gameObject.GetComponent<ParticleSystem>();
        bark = transform.Find("bark").gameObject;


        Cursor.lockState = CursorLockMode.Locked; // neskorej vyjebat
    }

    // Update is called once per frame
    void Update()
    {
        barkCounting -= Time.deltaTime;
        barkCooldownCounting -= Time.deltaTime;
        Bark();
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
            copyTerrain();
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
        if (Direction.magnitude > 0.01f || Input.GetMouseButton(1))
        {
            if (Direction.magnitude > 0.01f) anim.SetBool("isRunning", true);
            else anim.SetBool("isRunning", false);

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
        if (Physics.Raycast(frontPoint.transform.position, frontPoint.transform.TransformDirection(Vector3.down), out Fhit, 0.8f) && Physics.Raycast(backPoint.transform.position, backPoint.transform.TransformDirection(Vector3.down), out Bhit, 0.8f))
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
        } 
        else
        {
            Vector3 vector = model.transform.eulerAngles;
            vector.z = 0;
            model.transform.eulerAngles = vector;
        }
    }
    void Bark()
    {
        if (Input.GetKeyDown(KeyCode.E) && barkCooldownCounting < 0)
        {
            bark.SetActive(true);
            barkParticles.Play(true);
            barkCounting = barkTime;
            barkCooldownCounting = barkCoolDown;
        }

        if (barkCounting < 0)
        {
            bark.SetActive(false);
        }

       
    }
    private void OnDisable()
    {
        playerInputs.Disable();
    }
}

