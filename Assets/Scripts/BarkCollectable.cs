using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkCollectable : MonoBehaviour
{
    Transform colSpawnPoint;
    [SerializeField] GameObject collectable;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        colSpawnPoint = transform.Find("CollectableSpawnPoint").transform;
        player = GameObject.Find("Player-Dunco");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BarkCollider"))
        {
            GameObject coll = Instantiate(collectable, colSpawnPoint.position + Vector3.up * 2f, Quaternion.Euler(0, 360f * Random.value, 0));
            coll.GetComponent<CollectableEscape>().CreateTargetDir(player.transform.position);
        }
    }
}
