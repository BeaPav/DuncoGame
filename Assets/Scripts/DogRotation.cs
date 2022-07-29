using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogRotation : MonoBehaviour
{
    public GameObject front, back;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit fHit;
        RaycastHit bHit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fronttransform.position, transform.TransformDirection(Vector3.down), out fHit,Mathf.Infinity) && Physics.Raycast(back.transform.position, transform.TransformDirection(Vector3.down),out bHit, Mathf.Infinity)) ;
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }


        //vector3 upright = Vector3.Cross(transform.right, -(fHit.point - bHit.point).normalized);
        //transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, upright));
    }
}
