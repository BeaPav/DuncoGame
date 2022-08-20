using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gate : MonoBehaviour
{

    public GameObject dog;
    public GameObject barkText;
    public GameObject soulText;

    public GameObject Gate;

    public float distance;

    public int soulCount;

    public PlayerScoreScript PlayerScoreScript;

    private TextMeshProUGUI soulTextText;
    // Update is called once per frame

    private void Start()
    {
        soulTextText = soulText.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        soulTextText.text = PlayerScoreScript.noCollectables.ToString() + "/" + soulCount.ToString();

        if (PlayerScoreScript.noCollectables >= soulCount)
        {
            soulTextText.text = "Štek";
        }

        if((Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, dog.transform.position) < distance && PlayerScoreScript.noCollectables >= soulCount) || 
            Input.GetKeyDown(KeyCode.L)) 
        {
            Gate.SetActive(false);
            soulText.SetActive(false);
        }
    }
}
