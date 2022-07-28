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
            //Debug.Log("TriggerHeal");
            transform.parent.transform.GetComponent<SheepEnemy>().state = SheepState.Healthy;

            //zmena farby ovce
            transform.parent.Find("Sheep").GetComponent<Renderer>().material.SetColor("_Color",new Color(1f, 1f, 0.8f, 1f));
        }
    }
}
