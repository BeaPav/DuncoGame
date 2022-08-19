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
    float startDamageTime = 0f;


    [SerializeField] GameObject collectable;
    //GameObject colParent;
    Transform colSpawnPoint;


    private float startBounceTime = 0f;
    [SerializeField] Vector3 bounce = Vector3.zero;
    [SerializeField] float bounceSpeed;


    [SerializeField] AudioSource audioCollectable;



    // Start is called before the first frame update
    void Start()
    {
        noCollectables = 0;
        noCollectablesText = noCollectablesText.GetComponent<TextMeshProUGUI>();

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

        noCollectablesText.text = noCollectables.ToString();
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
        /*
        else if (other.CompareTag("DamageStoneRising"))
        {
            Damage(1, other.transform.parent.position);
        }
        */

        else if(other.CompareTag("Projectile"))
        {
            Damage(1, other.transform.parent.position);
        }


        else if(other.tag == "Collectable")
        {
            Destroy(other.gameObject);
            audioCollectable.Play();
            noCollectables++;
        }


        /*
        else if(other.CompareTag("Bounce"))
        {
            BounceDown();
            Debug.Log("BounceInBounce");
        }
        
        */
    }



    private void Damage(int noDam, Vector3 enemyPos)
    {
        if (Time.time - startDamageTime > 1f)
        {
            startDamageTime = Time.time;
            bounce = (transform.position - enemyPos).normalized;
            bounce.y = charControler.isGrounded ? 1.5f : 0f;


            DamageMaterial.SetColor("_Color", damageColor);
            startBounceTime = Time.time;

            if (noCollectables > 0)
            {
                noCollectables--;
                noCollectablesText.text = noCollectables.ToString();
                GameObject coll = Instantiate(collectable, colSpawnPoint.position + Vector3.up * 2f, Quaternion.Euler(0, 360f * Random.value, 0)
                                              );//,colParent.transform);

                //Debug.Log("spawn point: " + colSpawnPoint.position);
                //Debug.Log("collect pos: " + coll.transform.position);
                //Debug.Log("player pos: " + transform.position);

                coll.GetComponent<CollectableEscape>().CreateTargetDir(transform.position);
                //coll.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
            }
        }
        
    }


    public void BounceDown()
    {
        startBounceTime = Time.time;
        bounce = transform.right * 1f;
        bounce.y = 2f;
    }

}
