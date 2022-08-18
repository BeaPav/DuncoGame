using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDead : MonoBehaviour
{
    public SaveManager saveManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            saveManager.Restart();
        }
    }
}
