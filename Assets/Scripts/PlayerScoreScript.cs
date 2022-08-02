using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreScript : MonoBehaviour
{
    public TextMeshProUGUI noCollectablesText;
    private float startDamageTime = 0f;
    public int noCollectables = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startDamageTime > 1f)
        {
            transform.Find("pes/Damage").gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TriggerColliderSheepDamage");
        if (other.tag == "DamageSound")
        {
            Debug.Log("SoundDamage");
            Damage(1f);
        }
        else if(other.tag == "Collectable")
        {
            noCollectables++;
            noCollectablesText.text = noCollectables.ToString();
        }

    }


    private void Damage(float noDam)
    {
        transform.Find("pes/Damage").gameObject.SetActive(true);
        startDamageTime = Time.time;
    }
}
