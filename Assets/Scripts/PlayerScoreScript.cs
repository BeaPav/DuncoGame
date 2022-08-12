using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerScoreScript : MonoBehaviour
{
    public TextMeshProUGUI noCollectablesText;
    public int noCollectables;

    CharacterController charControler;

    
    Renderer DamageRender;
    Material DamageMaterial;
    Color startColor;
    Color damageColor;

    [SerializeField] GameObject collectable;
    //GameObject colParent;
    Transform colSpawnPoint;


    private float startBounceTime = 0f;
    [SerializeField] Vector3 bounce = Vector3.zero;
    [SerializeField] float bounceSpeed;




    // Start is called before the first frame update
    void Start()
    {
        noCollectables = 0;
        noCollectablesText = GameObject.Find("Canvas/Text (TMP)").GetComponent<TextMeshProUGUI>();

        charControler = GetComponent<CharacterController>();

        DamageRender = transform.Find("pivot/Dunco/Cube.002").GetComponent<SkinnedMeshRenderer>();
        DamageMaterial = DamageRender.materials[1];
        startColor = DamageMaterial.color;
        damageColor = new Color(214f / 255, 7f / 255, 197f / 255, 1f);
        colSpawnPoint = transform.Find("pivot/Dunco/CollectableSpawnPoint").transform;
        //colParent = GameObject.Find("CollectableParent").gameObject;

        bounceSpeed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startBounceTime > 0.5f)
        {
            
            DamageMaterial.SetColor("_Color", startColor);
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
        
        
        DamageMaterial.SetColor("_Color", damageColor);
        startBounceTime = Time.time;

        if (noCollectables > 0)
        {
            noCollectables--;
            noCollectablesText.text = noCollectables.ToString();
            GameObject coll = Instantiate(collectable, colSpawnPoint.position + Vector3.up * 1.5f, Quaternion.Euler(0, 360f * Random.value, 0)
                                          );//,colParent.transform);

            coll.GetComponent<CollectableEscape>().CreateTargetDir(transform.position);
            coll.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
        }
        
    }


    private void BounceDown()
    {
        startBounceTime = Time.time;
        bounce = transform.right * 1f;
        bounce.y = 3f;
    }

}
