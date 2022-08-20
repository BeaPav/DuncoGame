using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class EndMenu : MonoBehaviour
{
    public int MaxSouls;

    public TextMeshProUGUI duseText;
    private void Start()
    {
        duseText.text = PlayerPrefs.GetInt("duse").ToString() + "/ " + MaxSouls.ToString();
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Menu() 
    {
        SceneManager.LoadScene(0);
    }

    public void Again()
    {
        SceneManager.LoadScene(1);
    }
}
