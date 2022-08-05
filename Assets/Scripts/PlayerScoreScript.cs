using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerScoreScript : MonoBehaviour
{
    public TextMeshProUGUI noCollectablesText;
    private float startBounceTime = 0f;
    public int noCollectables;

    CharacterController charControler;

    
    Renderer DamageRender;
    Color startColor;
    Color damageColor;

    [SerializeField] Vector3 bounce = Vector3.zero;
    [SerializeField] float bounceSpeed;

    // Start is called before the first frame update
    void Start()
    {
        noCollectables = 0;

        charControler = GetComponent<CharacterController>();

        DamageRender = transform.Find("pivot/test/Cube.001").GetComponent<Renderer>();
        startColor = DamageRender.material.color;
        damageColor = new Color(214f / 255, 7f / 255, 197f / 255, 1f);


        bounceSpeed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startBounceTime > 0.5f)
        {
            
            DamageRender.material.SetColor("_Color", startColor);
            bounce = Vector3.zero;
        }

        if (bounce != Vector3.zero)
        {
            //Debug.Log("nonZeroBounce");
            charControler.Move(bounce * bounceSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TriggerColliderSheepDamage");
        if (other.tag == "DamageSound")
        {
            //Debug.Log("SoundDamage");
            Damage(1, other.transform.parent.position);
        }
        else if (other.CompareTag("DamageStone"))
        {
            //Debug.Log("StoneDamageColliderTriggered");
            if (charControler.isGrounded)
            {
                Damage(1, other.transform.parent.position);
            }
        }
        else if(other.CompareTag("Projectile"))
        {
            Damage(1, other.transform.parent.position);
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
            BounceDown();
            other.transform.parent.parent.GetComponent<SheepEnemy>().Heal();
            other.gameObject.tag = "Bounce";
        }
        else if (other.tag == "StoneHeal")
        {
            //Debug.Log("StoneHealColliderTriggered");
            noCollectables++;
            noCollectablesText.text = noCollectables.ToString();
            BounceDown();
            other.transform.parent.parent.GetComponent<StoneEnemy>().Heal();
            other.gameObject.SetActive(false);
        }
        else if(other.CompareTag("Bounce"))
        {
            BounceDown();
        }
        

    }


    private void Damage(int noDam, Vector3 enemyPos)
    {
        bounce = (transform.position - enemyPos).normalized;
        bounce.y = charControler.isGrounded? 1.5f : 0f;
        
        
        DamageRender.material.SetColor("_Color", damageColor);
        startBounceTime = Time.time;


    }


    private void BounceDown()
    {
        startBounceTime = Time.time;
        bounce = transform.right * 0.5f;
        bounce.y = 0f;
    }

}
