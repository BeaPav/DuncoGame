using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baca : MonoBehaviour
{
    public GameObject PressFText;
    public GameObject Dunco;
    public chat chat;

    public string[] who; 
    public string[] what;

    int index = -1;

    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(PressFText.transform.position, Dunco.transform.position) < distance)
        {
            PressFText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                index++;
                if (index == who.Length)
                {
                    chat.Hide();
                    index = -1;
                }
                else
                {
                    chat.SetText(who[index], what[index]);
                }
            }
        }
        else
        {
            PressFText.SetActive(false);
            chat.Hide();
        }
    }
}
