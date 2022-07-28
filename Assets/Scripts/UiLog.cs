using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiLog : MonoBehaviour
{
    public RectTransform contentHolder;
    public TextMeshProUGUI logText;
    public GameObject logCanvas;
    bool uiShowed = false;

    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    void LogCallback(string logString, string stackTrace, LogType type)
    {
        logText.text += $"[{System.DateTime.UtcNow.ToString("HH:mm")}] {logString} \r\n";
        contentHolder.sizeDelta = new Vector2(contentHolder.sizeDelta.x, contentHolder.sizeDelta.y + logText.fontSize * 1.15f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            if (!uiShowed)
            {
                uiShowed = true;
                logCanvas.active = true;
                Cursor.lockState = CursorLockMode.Confined;

                return;
            }
            uiShowed = false;
            logCanvas.active = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetKey(KeyCode.P))
        {
            Debug.Log("kokot");
        }
        
    }
}
