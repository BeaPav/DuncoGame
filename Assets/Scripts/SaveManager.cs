using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject dog;
    public PlayerScoreScript PlayerScoreScript;
    private int duseSave;
    private int lastChceckPointId = -1;
    private Vector3 savePosition;
    private CharacterController dogCharacterController;

    private void Start()
    {
        dogCharacterController = dog.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Restart();
        }
    }

    public void Save(int chceckPointId, Vector3 position)
    {
        if (chceckPointId > lastChceckPointId)
        {
            duseSave = PlayerScoreScript.noCollectables;
            savePosition = position;
        }
    }
    public void Restart()
    {
       /* SheepEnemy[] components = GameObject.FindObjectsOfType<SheepEnemy>();
        foreach(sheep comp in SheepEnemy)
            sheep.
        */
        
        GameObject[] duse = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (GameObject dusa in duse)
        {
            Destroy(dusa);
        }
        
        PlayerScoreScript.noCollectables = duseSave;
        dogCharacterController.enabled = false;
        dogCharacterController.transform.position = savePosition;
        dogCharacterController.enabled = true;
        Debug.Log(savePosition);
        Debug.Log(dogCharacterController.transform.position);
        Debug.Log("----");
    }
}
