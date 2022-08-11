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

    [SerializeField] float healOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        noCollectables = 0;

        charControler = GetComponent<CharacterController>();

        DamageRender = transform.Find("pivot/Dunco/Cube.002").GetComponent<Renderer>();
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
            if (false)
            {
                noCollectables++;
                //noCollectablesText.text = noCollectables.ToString();

                //other.gameObject.tag = "Bounce";
                //other.transform.parent.parent.GetComponent<SheepEnemy>().Heal();
            }

            //Debug.Log("Bounce");
            BounceDown();
            other.transform.parent.parent.GetComponent<Animator>().SetTrigger("isHit");
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
        else if (other.tag == "ZombieHeal")
        {
            if (false)
            {
                noCollectables++;
                //noCollectablesText.text = noCollectables.ToString();

                //other.gameObject.tag = "Bounce";
                //other.transform.parent.parent.GetComponent<SheepEnemy>().Heal();
            }

            //Debug.Log("Bounce");
            BounceDown();
            other.transform.parent.parent.GetComponent<Animator>().SetTrigger("isHit");
        }

        else if(other.CompareTag("Bounce"))
        {
            BounceDown();
            Debug.Log("BounceInBounce");
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
        bounce = transform.right * 1f;
        bounce.y = 3f;
    }

    private bool HealControl(Collider col)
    {
        if(col.transform.position.y < transform.position.y)
        {
            if(Mathf.Abs(col.transform.position.z - transform.position.z) < healOffset &&
               Mathf.Abs(col.transform.position.x - transform.position.x) < healOffset)
            {
                return true;
            }
        }
        return false;
    }

}
