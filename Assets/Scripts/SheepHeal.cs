using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepHeal : MonoBehaviour
{
    [SerializeField] GameObject SheepParent;

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
            SheepParent.GetComponent<SheepEnemy>().state = SheepState.Healthy;

            //vyliecenie ovce
            GameObject SheepMesh = SheepParent.transform.Find("Sheep").gameObject;
            SheepMesh.GetComponent<Renderer>().material.SetColor("_Color",new Color(1f, 1f, 0.8f, 1f));
            SheepMesh.GetComponent<Animator>().SetBool("isHealthy", true);
        }
    }
}
