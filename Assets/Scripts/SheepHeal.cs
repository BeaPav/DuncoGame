using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepHeal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("TriggerHeal");
            transform.parent.GetComponent<SheepEnemy>().isCursed = false;
            transform.parent.GetComponent<Renderer>().material.SetColor("_Color",new Color(1f, 1f, 0.8f, 1f));
        }
    }
}
