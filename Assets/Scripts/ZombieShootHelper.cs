using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieShootHelper : MonoBehaviour
{
    //[SerializeField]  GameObject zombieParent;
    [SerializeField] ZombieEnemy script;
    
    // Start is called before the first frame update
    void Start()
    {
        //zombieParent = transform.parent.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootHelper()
    {
        //zombieParent.GetComponent<ZombieEnemy>().Shoot();
        script.Shoot();
    }
}
