using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBillboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 180, 0f);
    }
}
