using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        Events.onHealthChanged.AddListener(ChangeHealth);
        Events.onPurpleChanged.AddListener(ChangePurple);
    }

    private void ChangeHealth(int health)
    {
        text.text = "health: " + health;
    }

    private void ChangePurple(float p)
    {
        text.text = "purple: " + p;
    }
}
