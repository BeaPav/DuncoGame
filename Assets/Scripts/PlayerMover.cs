using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float MoveSpeed;
    public CharacterController CharControl;

    public Transform Camera;

    public float SmoothTurnTime;
    float SmoothTurnVelocity;

    public float JumpSpeed;
    float YSpeed;


    private void Start()
    {
        MoveSpeed = 3f;
        SmoothTurnTime = 0.2f;
        JumpSpeed = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        Vector3 Direction = new Vector3(Horizontal, 0, Vertical).normalized;

        if(Direction.magnitude >= 0.1f)
        {

            //pohyb, ze mys kontroluje aj pohyb postavy
            float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref SmoothTurnVelocity, SmoothTurnTime);

            transform.rotation = Quaternion.Euler(0, SmoothAngle, 0);
            Vector3 MoveDirection = Quaternion.Euler(0, TargetAngle, 0) * Vector3.forward;
            MoveDirection = MoveDirection.normalized * MoveSpeed;
            
            YSpeed += Physics.gravity.y * Time.deltaTime;

            if (CharControl.isGrounded)
            {
                YSpeed = -0.5f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
               YSpeed = JumpSpeed;
            }

            
            

            MoveDirection.y = YSpeed;

            CharControl.Move(MoveDirection * Time.deltaTime);
            

            
        }
    }


}

