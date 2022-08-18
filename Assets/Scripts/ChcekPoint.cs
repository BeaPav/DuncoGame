using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChcekPoint : MonoBehaviour
{
    public int Id;

    private GameObject Dog;
    private SaveManager SaveManager;
    private bool used = false;

    public float Distance;
    // Start is called before the first frame update
    void Start()
    {
        Dog = GameObject.FindGameObjectWithTag("Player");
        SaveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position,Dog.transform.position) < Distance && !used)
        {
            SaveManager.Save(Id,transform.position);
            used = true;
        }
    }
}
