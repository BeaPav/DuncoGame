using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    Animator anim;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponent<Animator>();
        player = GameObject.Find("Player-Dunco");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("isFalling");
        }
    }
}
