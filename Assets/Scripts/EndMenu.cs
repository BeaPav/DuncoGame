using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndMenu : MonoBehaviour
{
    public void Menu() 
    {
        SceneManager.LoadScene(0);
    }

    public void Again()
    {
        SceneManager.LoadScene(1);
    }
}
