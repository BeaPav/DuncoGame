using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectWriter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mText;
    public float noCollPurple = 0f;
    public float noCollRed = 0f;

    private int health = 10;

    public void SetHealth(int h) 
    {
        health = h;
        Events.onHealthChanged.Invoke(health);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetColPurple(float p)
    {
        noCollPurple = p;
        Events.onPurpleChanged.Invoke(noCollPurple);
    }

    public void SetColRed(float r)
    {
        noCollRed = r;
    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            SetHealth(5);
        }

    }
}
