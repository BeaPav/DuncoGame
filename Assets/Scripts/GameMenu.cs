using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenu : MonoBehaviour
{
    bool isShow = false;
    public GameObject esc;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isShow = !isShow; 
            esc.active = isShow;

            if(isShow)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void Resume()
    {
        isShow = !isShow;
        esc.active = isShow;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
