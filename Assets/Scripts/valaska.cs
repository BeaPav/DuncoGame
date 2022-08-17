using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class valaska : MonoBehaviour
{
    public float distance;
    public GameObject dog;
    private void FixedUpdate()
    {
        if(Vector3.Distance(dog.transform.position,transform.position) < distance)
        {
            SceneManager.LoadScene(2);
        }
    }
}
