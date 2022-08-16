using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class chat : MonoBehaviour
{
    public GameObject chatObj;

    public TextMeshProUGUI whoDisplay;
    public TextMeshProUGUI whatDisplay;

    public void SetText(string who, string what)
    {
        chatObj.SetActive(true);

        whoDisplay.text = who;
        whatDisplay.text = what;
    }

    public void Hide()
    {
        chatObj.SetActive(false);
    }
}
