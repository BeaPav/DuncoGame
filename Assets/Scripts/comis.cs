using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class comis : MonoBehaviour
{

    public Image img;
    public Sprite[] imgs;
    private int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        img.sprite = imgs[id];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            id++;

            if(id == imgs.Length)
            {
                SceneManager.LoadScene(2);
            }

            img.sprite = imgs[id];
        }
    }
}
