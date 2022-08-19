using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkCollectable : MonoBehaviour
{
    Transform colSpawnPoint;
    [SerializeField] GameObject collectable;
    bool hasCollectable;

    GameObject player;

    [SerializeField] List<GameObject> objectsNotToStopMovement;

    GameObject particles;
    GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        colSpawnPoint = transform.Find("CollectableSpawnPoint").transform;
        player = GameObject.Find("Player-Dunco");
        hasCollectable = true;
        objectsNotToStopMovement.Add(transform.Find("DuchHideMesh").gameObject);
        particles = transform.Find("Duch_particle").gameObject;
        light = transform.Find("SpotLight").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BarkCollider") && hasCollectable)
        {
            hasCollectable = false;
            Destroy(particles);
            Destroy(light);
            GameObject coll = Instantiate(collectable, colSpawnPoint.position + Vector3.up * 1f, Quaternion.Euler(0, 360f * Random.value, 0));
            coll.GetComponent<CollectableEscape>().objectsNotToStopMovement = objectsNotToStopMovement;
            coll.GetComponent<CollectableEscape>().CreateTargetDir(player.transform.position);
        }
    }
}
