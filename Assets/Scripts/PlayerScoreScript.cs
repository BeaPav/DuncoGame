using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreScript : MonoBehaviour
{
    public TextMeshProUGUI noCollectablesText;
    private float startDamageTime = 0f;
    public int noCollectables;

    CharacterController charControler;

    // Start is called before the first frame update
    void Start()
    {
        noCollectables = 0;
        charControler = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startDamageTime > 0.8f)
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
            Damage(1);
        }
        else if (other.CompareTag("DamageStone"))
        {
            //Debug.Log("StoneDamageColliderTriggered");
            if (charControler.isGrounded)
            {
                Damage(1);
            }
        }
        else if(other.tag == "Collectable")
        {
            other.gameObject.SetActive(false);
            noCollectables++;
            noCollectablesText.text = noCollectables.ToString();
        }
        else if (other.tag == "SheepHeal")
        {
            noCollectables++;
            noCollectablesText.text = noCollectables.ToString();
            other.transform.parent.parent.GetComponent<SheepEnemy>().Heal();
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "StoneHeal")
        {
            //Debug.Log("StoneHealColliderTriggered");
            noCollectables++;
            noCollectablesText.text = noCollectables.ToString();
            other.transform.parent.parent.GetComponent<StoneEnemy>().Heal();
            other.gameObject.SetActive(false);
        }
        

    }


    private void Damage(int noDam)
    {
        transform.Find("pes/Damage").gameObject.SetActive(true);
        startDamageTime = Time.time;
    }
}
