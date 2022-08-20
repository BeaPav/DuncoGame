using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class valaska : MonoBehaviour
{
    public float timer;
    public float distance;
    public GameObject dog;

    private void FixedUpdate()
    {
        if(Vector3.Distance(dog.transform.position,transform.position) < distance)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        PlayerPrefs.SetInt("duse",dog.GetComponent<PlayerScoreScript>().noCollectables);
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(3);
    }
}
