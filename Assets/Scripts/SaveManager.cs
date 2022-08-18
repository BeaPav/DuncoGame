using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameObject dog;
    public PlayerScoreScript PlayerScoreScript;
    private int duseSave;
    private int lastChceckPointId;
    private Vector3 savePosition;
    public GameObject[] chceckPoints;
    private void Start()
    {
        Restart();
    }

    public void Save(int chceckPointId)
    {
        if (chceckPointId > lastChceckPointId)
        {
            duseSave = PlayerScoreScript.noCollectables;
            savePosition = chceckPoints[chceckPointId - 1].transform.position;
        }
    }
    public void Restart()
    {
        GameObject[] duse = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (GameObject dusa in duse)
        {
            Destroy(dusa);
        }
        
        PlayerScoreScript.noCollectables = duseSave;
        dog.transform.position = savePosition;
    }
}
